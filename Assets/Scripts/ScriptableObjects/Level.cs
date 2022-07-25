using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "GameData/Level")]
public class Level : ScriptableObject
{
    public string levelName;

    public GameObject playAreaChunkPrefab;

    public int garbageTotalCount;
    public List<GameObject> garbagePrefabs;

    public int commonEnvironmentCount;
    public List<GameObject> environmentCommonPrefabs;

    public int eyeCandyEnvironmentCount;
    public List<GameObject> environmentEyeCandyPrefabs;
}
