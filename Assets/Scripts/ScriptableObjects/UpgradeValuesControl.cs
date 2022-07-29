using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeValuesControl", menuName = "GameData/UpgradeControl")]
public class UpgradeValuesControl : ScriptableObject
{
    public float movementSpeedChange;
    public int capacityChange;
    public float vacuumStrengthChange;
}
