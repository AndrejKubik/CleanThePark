using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData
{
    public int currentLevel;
    public int moneyTotal;
    public float wheelSpeed;
    public float speed;
    public int capacity;
    public int vacuumWidth;
    public float pullSpeed;

    public SaveGameData()
    {
        currentLevel = GameManager.currentLevel;
        moneyTotal = GameManager.moneyTotal;
        wheelSpeed = GameManager.wheelSpeed;
        speed = GameManager.speed;
        capacity = GameManager.capacity;
        vacuumWidth = GameManager.vacuumWidth;
        pullSpeed = GameManager.pullSpeed;
    }
}
