using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData
{
    public int currentLevel;
    public int moneyTotal;

    public int speedLevel;
    public float wheelSpeed;
    public float speed;
    public int capacity;
    public int vacuumLevel;
    public float pullSpeed;

    public int speedUpgCost;
    public int vacuumUpgCost;
    public int capacityUpgCost;

    public int speedUpgCount;
    public int vacuumUpgCount;
    public int capacityUpgCount;

    public SaveGameData()
    {
        currentLevel = GameManager.currentLevel;
        moneyTotal = GameManager.moneyTotal;

        speedLevel = GameManager.speedLevel;
        wheelSpeed = GameManager.wheelSpeed;
        speed = GameManager.speed;
        capacity = GameManager.capacity;
        vacuumLevel = GameManager.vacuumLevel;
        pullSpeed = GameManager.pullSpeed;

        speedUpgCost = UIController.speedUpgradeCost;
        vacuumUpgCost = UIController.vacuumUpgradeCost;
        capacityUpgCost = UIController.capacityUpgradeCost;

        speedUpgCount = UIController.speedUpgCount;
        vacuumUpgCount = UIController.vacuumUpgCount;
        capacityUpgCount = UIController.capacityUpgCount;
    }
}
