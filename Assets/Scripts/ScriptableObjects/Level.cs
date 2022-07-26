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

    public List<GameObject> environmentCommonPrefabs;

    public List<GameObject> environmentEyeCandyPrefabs;
}
