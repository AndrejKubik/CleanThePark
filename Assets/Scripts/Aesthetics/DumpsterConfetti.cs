using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpsterConfetti : MonoBehaviour
{
    public void PlayMoneyConfetti()
    {
        GameManager.instance.deliveryParticles.SetActive(true); //play the delivery particle effect
        SoundManager.instance.PlayDeliverySound(); //play the cha-ching sound

        if (GameManager.instance.levelClear) //if the level has been cleared
        {
            SoundManager.instance.PlayLevelEndSound(); //play the victory sound
            GameManager.instance.levelFinished = true; //change the game state

            AnalyticsManagerr.instance.LevelWin(); //trigger the analytics level win event

            GameManager.instance.SaveGame(); //save the game data

            if (GameManager.currentLevel < 30) UIController.instance.victoryMenu.SetActive(true); //if the level is done, show the victory screen
            else if (GameManager.currentLevel == 30) UIController.instance.endScreen.SetActive(true); //if it is the final level show the end game screen instead
        }
    }
}
