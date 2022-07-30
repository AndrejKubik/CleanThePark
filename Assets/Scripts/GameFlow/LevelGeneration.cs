using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private Transform garbageRegionsParent;
    private List<Collider> garbageRegions;

    public List<Vector3> usedPositions;

    private List<GameObject> garbagePrefabs;
    private int garbageCount;

    private List<GameObject> environmentPrefabs;
    [SerializeField] private Transform commonEnvironmentParent;
    private List<Transform> environmentSpawnPositions;

    private List<GameObject> eyeCandyPrefabs;
    [SerializeField] private Transform eyeCandyParent;
    private List<Transform> eyeCandySpawnPositions;

    private float spawnHeight = 2f;


    private void Start()
    {
        LoadLevelData();
        Debug.Log("garbage to spawn: " + garbageCount);

        environmentSpawnPositions = new List<Transform>();
        AddAllChildrenToList(commonEnvironmentParent, environmentSpawnPositions); 

        eyeCandySpawnPositions = new List<Transform>();
        AddAllChildrenToList(eyeCandyParent, eyeCandySpawnPositions);

        garbageRegions = new List<Collider>();
        AddAllChildrenCollidersToList(garbageRegionsParent, garbageRegions);

        GenerateLevel();
        GameManager.instance.levelStarted = true;
        Debug.Log("garbage spawned: " + GameManager.instance.garbageCount);
    }

    public void GenerateLevel()
    {
        GenerateGarbage(garbagePrefabs, garbageCount, garbageRegions); //spawn a random garbage prefab within the garbage spawn region until chosen ammount of garbage is reached

        GenerateEnvironment(environmentPrefabs, environmentSpawnPositions.Count, environmentSpawnPositions); //spawn a random environment prefab on every empty environment spot

        GenerateEyeCandy(eyeCandyPrefabs, eyeCandySpawnPositions.Count, eyeCandySpawnPositions); //spawn a random eye candy environment prefab on every empty empty spot
    }

    private void GenerateEnvironment(List<GameObject> objectsToSpawn, int numberOfObjects, List<Transform> positions)
    {
        for (int i = 0; i < numberOfObjects; i++) //for every object to spawn
        {
            int randomIndex = Random.Range(0, positions.Count); //choose a random element from the position list
            Vector3 randomSpawnPosition = positions[randomIndex].position; //get the random spawn position
            positions.RemoveAt(randomIndex); //remove the used position from the list

            float randomY = Random.Range(0f, 180f); //get the random Y-coordinate for the spawn rotation quaternion
            Vector3 randomRotation = new Vector3(0f, randomY, 0f); //make the vector for the random rotation quaternion

            randomIndex = Random.Range(0, objectsToSpawn.Count); //choose a random element from the prefabs list
            Instantiate(objectsToSpawn[randomIndex], randomSpawnPosition, Quaternion.Euler(randomRotation), GameManager.instance.environmentParent); //spawn the chosen object at the gotten position
        }
    }

    private void GenerateEyeCandy(List<GameObject> objectsToSpawn, int numberOfObjects, List<Transform> positions)
    {
        for (int i = 0; i < numberOfObjects; i++) //for every object to spawn
        {
            int randomIndex = Random.Range(0, positions.Count); //choose a random element from the position list
            Transform randomSpawnPosition = positions[randomIndex]; //get the random spawn position
            positions.RemoveAt(randomIndex); //remove the used position from the list

            randomIndex = Random.Range(0, objectsToSpawn.Count); //choose a random element from the prefabs list
            Instantiate(objectsToSpawn[randomIndex], randomSpawnPosition.position, randomSpawnPosition.rotation, GameManager.instance.eyeCandyParent); //spawn the chosen object as a child of an empty spawn position object
        }
    }

    private void GenerateGarbage(List<GameObject> objectsToSpawn, int numberOfObjects, List<Collider> spawnRegions)
    {
        for(int i = 0; i < spawnRegions.Count; i++) //for every spawn region
        {
            for (int j = 0; j < (numberOfObjects / spawnRegions.Count); j++) //spread the total count of garbage among all regions evenly
            {
                float randomY = Random.Range(0f, 180f); //get the random Y-coordinate for the spawn rotation quaternion
                Vector3 randomRotation = new Vector3(0f, randomY, 0f); //make the vector for the random rotation quaternion

                Vector3 randomSpawnPosition = RandomPosition(spawnRegions[i]); //get the random position in the chosen region

                int randomIndex = Random.Range(0, objectsToSpawn.Count); //choose a random element from the prefabs list
                Instantiate(objectsToSpawn[randomIndex], randomSpawnPosition, Quaternion.Euler(randomRotation), GameManager.instance.garbageParent); //spawn the chosen object at the gotten position
                GameManager.instance.garbageCount++; //increment the objective trash count
            }
        }

        for(int i = 0; i < spawnRegions.Count; i++) //for every collider region
        {
            Destroy(spawnRegions[i].gameObject); //destroy the current region object
        }
    }

    public Vector3 RandomPosition(Collider region)
    {
        //get random Z and X axis values for the random position within the chosen area
        float xRandom = Random.Range(region.bounds.min.x, region.bounds.max.x);
        float zRandom = Random.Range(region.bounds.min.z, region.bounds.max.z);

        Vector3 randomSpawnPosition = new Vector3(xRandom, spawnHeight, zRandom); //combine the 2 gotten coordinates to get the final position Vector

        return randomSpawnPosition;
    }

    public void AddAllChildrenToList(Transform parent, List<Transform> children)
    {
        for(int i = 0; i < parent.childCount; i++) //add all chile objects from the parent to a list of transforms
        {
            children.Add(parent.GetChild(i));
        }
    }

    public void AddAllChildrenCollidersToList(Transform parent, List<Collider> children)
    {
        for (int i = 0; i < parent.childCount; i++) //add all chile objects from the parent to a list of colliders
        {
            children.Add(parent.GetChild(i).GetComponent<BoxCollider>());
        }
    }

    public void LoadLevelData()
    {
        garbagePrefabs = GameManager.instance.currentLevelData.garbagePrefabs;
        garbageCount = GameManager.instance.currentLevelData.garbageTotalCount;
        environmentPrefabs = GameManager.instance.currentLevelData.environmentCommonPrefabs;
        eyeCandyPrefabs = GameManager.instance.currentLevelData.environmentEyeCandyPrefabs;
    }

    #region Mercy M'Lord

    //public Vector3 RandomGarbagePosition()
    //{
    //    //get random Z and X axis values for the random position
    //    int xRandom = RandomGarbageX();
    //    int zRandom = RandomGarbageZ();

    //    Vector3 randomSpawnPosition = new Vector3(xRandom, 0f, zRandom); //combine the 2 gotten coordinates to get the final position Vector

    //    if (usedPositions.Contains(randomSpawnPosition)) return RandomGarbagePosition(); //if the calculated position Vector is already in use, try again
    //    else return randomSpawnPosition; //otherwise pass in the calculated Vector
    //}

    //public Vector3 RandomEnvironmentPosition()
    //{
    //    ////get random Z and X axis values for the random position
    //    //int xRandom = RandomEnvironmentX();
    //    //int zRandom = RandomEnvironmentZ();

    //    Vector3 randomSpawnPosition = RandomEnvironment();

    //    if (usedPositions.Contains(randomSpawnPosition)) return RandomEnvironmentPosition(); //if the calculated position Vector is already in use, try again
    //    else return randomSpawnPosition; //otherwise pass in the calculated Vector
    //}

    //private int RandomGarbageX()
    //{
    //    int randomX = (int)Random.Range(terrain.bounds.min.x + environmentSideRange, terrain.bounds.max.x - environmentSideRange); //calculate the random position coordinate within the terrain collider

    //    return randomX;
    //}

    //private int RandomGarbageZ()
    //{
    //    int randomZ = (int)Random.Range(terrain.bounds.min.z + environmentSideRange, terrain.bounds.max.z - environmentSideRange); //calculate the random position coordinate within the terrain collider

    //    return randomZ;
    //}

    //private int RandomEnvironmentX()
    //{
    //    int randomX1 = (int)Random.Range(terrain.bounds.min.x, terrain.bounds.min.x + environmentSideRange); //calculate the random position coordinate within the terrain collider
    //    int randomX2 = (int)Random.Range(terrain.bounds.max.x - environmentSideRange, terrain.bounds.max.x); //calculate the random position coordinate within the terrain collider

    //    int randomXref = Random.Range(0, 2);

    //    if (randomXref == 0) return randomX1;
    //    else return randomX2;
    //}

    //private int RandomEnvironmentZ()
    //{
    //    int randomZ1 = (int)Random.Range(terrain.bounds.min.z, terrain.bounds.min.z + environmentSideRange); //calculate the random position coordinate within the terrain collider
    //    int randomZ2 = (int)Random.Range(terrain.bounds.max.z - environmentSideRange, terrain.bounds.max.z); //calculate the random position coordinate within the terrain collider

    //    int randomZref = Random.Range(0, 2);

    //    if (randomZref == 0) return randomZ1;
    //    else return randomZ2;
    //}

    //private Vector3 RandomEnvironment()
    //{
    //    //levi pravougaonik
    //    int randomX1 = (int)Random.Range(terrain.bounds.min.x, terrain.bounds.min.x + environmentSideRange);
    //    //desni pravougaonik
    //    int randomX2 = (int)Random.Range(terrain.bounds.max.x - environmentSideRange, terrain.bounds.max.x);
    //    int randomZ12 = (int)Random.Range(terrain.bounds.min.z, terrain.bounds.max.z);
    //    //gornji pravougaonik
    //    int randomX34 = (int)Random.Range(terrain.bounds.min.x + environmentSideRange, terrain.bounds.max.x - environmentSideRange);
    //    int randomZ3 = (int)Random.Range(terrain.bounds.max.z - environmentSideRange, terrain.bounds.max.z);
    //    //donji
    //    //int randomZ4 = (int)Random.Range(terrain.bounds.min.z, terrain.bounds.min.z + environmentSideRange);

    //    //izbor
    //    int option = (int)Random.Range(0, 3);
    //    int randomX;
    //    int randomZ;

    //    if (option == 0)
    //    {
    //        randomX = randomX1;
    //        randomZ = randomZ12;
    //    }
    //    else if (option == 1)
    //    {
    //        randomX = randomX2;
    //        randomZ = randomZ12;
    //    }
    //    else /*if (option == 2)*/
    //    {
    //        randomX = randomX34;
    //        randomZ = randomZ3;
    //    }
    //    //else
    //    //{
    //    //    randomX = randomX34;
    //    //    randomZ = randomZ4;
    //    //}

    //    return new Vector3(randomX, 0f, randomZ);
    //}
    #endregion
}
