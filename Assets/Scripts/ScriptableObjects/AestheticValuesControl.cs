using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAesthethicControl", menuName = "GameData/AesthethicsControl")]
public class AestheticValuesControl : ScriptableObject
{
    public float wheelSpeedStart;
    public float wheelSpeedChange;

    public float vacuumStrengthStart;
    public float vacuumStrengthChange;

    public float startMovementSpeed;

    public int startCapacity;
    public int numberOfTrashForStack;
}
