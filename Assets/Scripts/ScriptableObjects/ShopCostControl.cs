using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopControl", menuName = "GameData/ShopControl")]
public class ShopCostControl : ScriptableObject
{
    public int speedUpgradeStartCost;
    public int vacuumUpgradeStartCost;
    public int capacityUpgradeStartCost;

    public int speedUpgradeChange;
    public int vacuumUpgradeChange;
    public int capacityUpgradeChange;

    public int moneyGainStart;
    public int moneyGainChange;
}
