using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Collider garbageRegion;
    public List<Collider> environmentRegions;

    public List<Vector3> usedPositions;

    public List<GameObject> garbagePrefabs;
    public int garbageCount;

    public List<GameObject> environmentPrefabs;
    public int environmentCount;

    Vector3 randomSpawnPosition;

    //public float environmentSideRange;
    //public float environemntVerticalRange;

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        for (int i = 0; i < garbagePrefabs.Count; i++)
        {
            GenerateObject(garbagePrefabs[i], garbageCount, garbageRegion);
        }

        //for (int i = 0; i < environmentRegions.Count; i++)
        //{
        //    for (int j = 0; j < environmentPrefabs.Count; j++)
        //    {
        //        GenerateObject(environmentPrefabs[j], environmentCount, environmentRegions[i]);
        //    }
        //}
    }
    //private void GenerateGarbage(GameObject objectToSpawn, int numberOfObjects, Collider region)
    //{
    //    for (int i = 0; i < numberOfObjects; i++) //for every object to spawn
    //    {
    //        Vector3 randomSpawnPosition = RandomPosition(region); //get the random spawn position
    //        usedPositions.Add(randomSpawnPosition); //store it in the used list to prevent overlaping
    //        Instantiate(objectToSpawn, randomSpawnPosition, transform.rotation); //spawn the chosen object at the gotten position
    //    }
    //}

    //private void GenerateEnvironment(GameObject objectToSpawn, int numberOfObjects, Collider region)
    //{
    //    for (int i = 0; i < numberOfObjects; i++) //for every object to spawn
    //    {
    //        Vector3 randomSpawnPosition = RandomPosition(region); //get the random spawn position
    //        usedPositions.Add(randomSpawnPosition); //store it in the used list to prevent overlaping
    //        Instantiate(objectToSpawn, randomSpawnPosition, transform.rotation); //spawn the chosen object at the gotten position
    //    }
    //}

    private void GenerateObject(GameObject objectToSpawn, int numberOfObjects, Collider spawnRegion)
    {
        for (int i = 0; i < numberOfObjects; i++) //for every object to spawn
        {
            GetRandomPoint(spawnRegion);
            Instantiate(objectToSpawn, randomSpawnPosition, transform.rotation); //spawn the chosen object at the gotten position
        }
    }

    public Vector3 RandomPosition(Collider region)
    {
        //get random Z and X axis values for the random position within the chosen area
        float xRandom = Random.Range(region.bounds.min.x, region.bounds.max.x);
        float zRandom = Random.Range(region.bounds.min.z, region.bounds.max.z);

        Vector3 randomSpawnPosition = new Vector3(xRandom, 0f, zRandom); //combine the 2 gotten coordinates to get the final position Vector

        return randomSpawnPosition;
    }

    public void GetRandomPoint(Collider region)
    {
        RaycastHit hit;

        randomSpawnPosition = RandomPosition(region);

        if (Physics.Raycast(randomSpawnPosition, Vector3.down, out hit, 10f))
        {
            Vector3 randomPosition = new Vector3(hit.point.x, 0f, hit.point.z);

            //if (hit.collider.gameObject.layer == 3)
            //{
            //    Debug.Log("hit ground");
                
            //    if (!usedPositions.Contains(randomPosition))
            //    {
            //        randomSpawnPosition = randomPosition;
            //        usedPositions.Add(randomPosition); //store it in the used list to prevent overlaping
            //    }
            //    else if (hit.collider.gameObject.layer == 6)
            //    {
            //        Debug.Log("kurac");
            //        GetRandomPoint(region);
            //    }
            //}

            if(hit.collider.gameObject.layer == 6)
            {
                GetRandomPoint(region);
            }
            else
            {
                randomSpawnPosition = randomPosition;
            }
        }
    }

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
}
