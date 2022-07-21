using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManagerr : MonoBehaviour
{
    #region Singleton
    public static AnalyticsManagerr instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public string levelNumber;
    public void LevelStart()
    {
        levelNumber = GameManager.currentLevel.ToString(); //get the current level number
        TinySauce.OnGameStarted(levelNumber); //fire the event
        Debug.Log("Level " + levelNumber + " started!");
    }

    public void LevelWin()
    {
        levelNumber = GameManager.currentLevel.ToString(); //get the current level number
        TinySauce.OnGameFinished(true, GameManager.moneyTotal, levelNumber);
        Debug.Log("Level " + levelNumber + " beaten!");
    }
}
