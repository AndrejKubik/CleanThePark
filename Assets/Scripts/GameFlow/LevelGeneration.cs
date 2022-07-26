using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Collider garbageRegion;

    public List<Vector3> usedPositions;

    public List<GameObject> garbagePrefabs;
    private int garbageCount;

    public List<GameObject> environmentPrefabs;

    public List<Transform> environmentSpawnPositions;

    private void Start()
    {
        LoadLevelData();
        Debug.Log("garbage to spawn: " + garbageCount);
        GenerateLevel();
        GameManager.instance.levelStarted = true;
        Debug.Log("garbage spawned: " + GameManager.instance.garbageCount);
    }

    public void GenerateLevel()
    {
        //for (int i = 0; i < garbagePrefabs.Count; i++) //for every garbage prefab
        //{
        //    GenerateObject(garbagePrefabs[i], garbageCount, garbageRegion); //spawn the chosen ammount of the current prefab randomly in the chosen area
        //}

        GenerateGarbage(garbagePrefabs, garbageCount, garbageRegion); //spawn a random garbage prefab within the garbage spawn region until chosen ammount of garbage is reached

        GenerateEnvironment(environmentPrefabs, environmentSpawnPositions.Count, environmentSpawnPositions); //spawn a random environment prefab on every empty environment spot
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
            Instantiate(objectsToSpawn[randomIndex], randomSpawnPosition, Quaternion.Euler(randomRotation)); //spawn the chosen object at the gotten position
        }
    }

    private void GenerateGarbage(List<GameObject> objectsToSpawn, int numberOfObjects, Collider spawnRegion)
    {
        for(int i = 0; i < numberOfObjects; i++)
        {
            float randomY = Random.Range(0f, 180f); //get the random Y-coordinate for the spawn rotation quaternion
            Vector3 randomRotation = new Vector3(0f, randomY, 0f); //make the vector for the random rotation quaternion

            Vector3 randomSpawnPosition = RandomPosition(spawnRegion); //get the random position in the chosen region

            int randomIndex = Random.Range(0, objectsToSpawn.Count); //choose a random element from the prefabs list
            Instantiate(objectsToSpawn[randomIndex], randomSpawnPosition, Quaternion.Euler(randomRotation)); //spawn the chosen object at the gotten position
            GameManager.instance.garbageCount++; //increment the objective trash count
        }
    }

    private void GenerateObject(GameObject objectToSpawn, int numberOfObjects, Collider spawnRegion)
    {
        for (int i = 0; i < numberOfObjects; i++) //for every object to spawn
        {
            float randomY = Random.Range(0f, 180f); //get the random Y-coordinate for the spawn rotation quaternion
            Vector3 randomRotation = new Vector3(0f, randomY, 0f); //make the vector for the random rotation quaternion

            Vector3 randomSpawnPosition = RandomPosition(spawnRegion); //get the random position in the chosen region
            Instantiate(objectToSpawn, randomSpawnPosition, Quaternion.Euler(randomRotation)); //spawn the chosen object at the gotten position
            GameManager.instance.garbageCount++; //increment the objective trash count
        }
    }

    public Vector3 RandomPosition(Collider region)
    {
        //get random Z and X axis values for the random position within the chosen area
        float xRandom = Random.Range(region.bounds.min.x, region.bounds.max.x);
        float zRandom = Random.Range(region.bounds.min.z, region.bounds.max.z);

        Vector3 randomSpawnPosition = new Vector3(xRandom, 0.5f, zRandom); //combine the 2 gotten coordinates to get the final position Vector

        return randomSpawnPosition;
    }

    public void LoadLevelData()
    {
        garbagePrefabs = GameManager.instance.currentLevelData.garbagePrefabs;
        garbageCount = GameManager.instance.currentLevelData.garbageTotalCount;
        environmentPrefabs = GameManager.instance.currentLevelData.environmentCommonPrefabs;
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
