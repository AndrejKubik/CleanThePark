using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Singleton
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject startMenu;

    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private TextMeshProUGUI speedCostNumber;
    [SerializeField] private TextMeshProUGUI vacuumCostNumber;
    [SerializeField] private TextMeshProUGUI capacityCostNumber;

    public GameObject victoryMenu;
    public GameObject endScreen;

    public GameObject winText;

    [SerializeField] private GameObject joystick;

    public TextMeshProUGUI moneyCount;

    public static int timesSpeedUpgraded = 0;
    public static int timesVacuumUpgraded = 0;
    public static int timesCapacityUpgraded = 0;

    private int maxUpgrades = 2;
    private int speedCostChange = 40;
    private int vacuumCostChange = 100;
    private int capacityCostChange = 30;

    public static int speedUpgradeCost = 20;
    public static int vacuumUpgradeCost = 60;
    public static int capacityUpgradeCost = 25;

    public Animator speedAnim;
    public Animator vacuumAnim;
    public Animator capacityAnim;

    public GameObject speedMax;
    public GameObject vacuumMax;
    public GameObject capacityMax;

    private void Start()
    {
        UpdateShopUI();

        if (GameManager.scenesLoaded == 0) //if the application is just turned on
        {
            startMenu.SetActive(true); //show the start menu
            Time.timeScale = 0f; //freeze game time
            GameManager.scenesLoaded++; //increment the first load check
        }
        else
        {
            joystick.SetActive(true); //activate the joystick
            menuButton.SetActive(true); //show the pause button
            Time.timeScale = 1f; //resume game time
            startMenu.SetActive(false); //hide the start menu
        }
    }

    public void ResetShop()
    {
        //disable all text blockers for shop
        speedMax.SetActive(false);
        vacuumMax.SetActive(false);
        capacityMax.SetActive(false);

        //reset all upgrades
        timesSpeedUpgraded = 0;
        timesVacuumUpgraded = 0;
        timesCapacityUpgraded = 0;

        //reset all costs
        speedUpgradeCost = 20;
        vacuumUpgradeCost = 60;
        capacityUpgradeCost = 25;

        //reset the upgrade cost texts
        speedCostNumber.text = speedUpgradeCost.ToString();
        vacuumCostNumber.text = vacuumUpgradeCost.ToString();
        capacityCostNumber.text = capacityUpgradeCost.ToString();
    }

    public void StartGame()
    {
        startMenu.SetActive(false); //hide start menu
        joystick.SetActive(true); //activate the joystick
        menuButton.SetActive(true); //show the pause button
        Time.timeScale = 1f; //resume game time

        AnalyticsManagerr.instance.LevelStart(); //trigger the analytics level start event
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); //show pause menu
        joystick.SetActive(false); //deactivate the joystick
        menuButton.SetActive(false); //hide the pause button
        Time.timeScale = 0f; //pause game time
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false); //hide pause menu
        joystick.SetActive(true); //activate the joystick
        menuButton.SetActive(true); //show the pause button
        Time.timeScale = 1f; //resume game time
    }

    public void RestartGame()
    {
        GameManager.currentLevel = 1; //set the level number to 1
        Destroy(GameObject.FindGameObjectWithTag("Music")); //if the game is started over, start the music from the start
        AnalyticsManagerr.instance.LevelStart(); //trigger the analytics level start event

        SceneManager.LoadScene(0); //load the first level 
    }

    public void NextLevel()
    {
        GameManager.currentLevel++; //increment the level number
        GameManager.instance.SaveGame(); //save the game data
        AnalyticsManagerr.instance.LevelStart(); //trigger the analytics level start event
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reload the scene
    }

    public void UpgradeSpeed()
    {
        if (GameManager.moneyTotal >= speedUpgradeCost) //if the player has enough money
        {
            if (timesSpeedUpgraded < maxUpgrades) //if the stat is not at max level already
            {
                GameManager.speed += GameManager.instance.speedUpg; //increase the player's speed by the chose value
                GameManager.wheelSpeed += GameManager.instance.wheelSpeedUpg; //rotate wheels faster
                StartCoroutine(ShowPopUp(speedAnim.gameObject, speedAnim)); //show the text popup

                Debug.Log("Speed upgraded: " + GameManager.speed); //print current speed in the console

                BuyUpgrade(speedUpgradeCost); //do the transaction

                speedUpgradeCost += speedCostChange; //increase the cost by the set ammount for next upgrade
                speedCostNumber.text = speedUpgradeCost.ToString(); //update the text of the cost

                if (timesSpeedUpgraded == (maxUpgrades - 1)) speedMax.SetActive(true); //show the max level sign

                SoundManager.instance.PlayUpgSound(); //play the upgrade sound

                timesSpeedUpgraded++; //increment the counter for speed upgrades
            }
            else //if the player has the stat at max level
            {
                Debug.Log("Reached max speed already"); //print the max level warning in the console
            }
        }
        else //if the player does not have enough money
        {
            GameManager.instance.moneyAnimator.Play("MoneyNoBlob"); //play the no money ui animation
            SoundManager.instance.PlayNoMoneySound(); //play the no money sound
        }
    }

    public void UpgradeVacuum()
    {
        if (GameManager.moneyTotal >= vacuumUpgradeCost) //if the player has enough money
        {
            if (timesVacuumUpgraded < maxUpgrades) //if the stat is not at the max level
            {
                GameManager.instance.TurnVacuumOff(); //hide the current vacuum indicator
                GameManager.vacuumWidth++; //change the vacuum model to the next level model
                GameManager.pullSpeed += GameManager.instance.pullUpg; //increase the pulling strength

                StartCoroutine(ShowPopUp(vacuumAnim.gameObject, vacuumAnim)); //show the text popup

                Debug.Log("current vacuum: " + GameManager.vacuumWidth); //print the current vacuum number in the console

                if (GameManager.instance.canPull) GameManager.instance.TurnVacuumOn(); //if the player doesn't have max stacks on his back, show the next level vacuum indicator

                BuyUpgrade(vacuumUpgradeCost); //do the transaction

                vacuumUpgradeCost += vacuumCostChange; //increase the cost by the set ammount for next upgrade
                vacuumCostNumber.text = vacuumUpgradeCost.ToString(); //update the text of the cost

                if (timesVacuumUpgraded == (maxUpgrades - 1)) vacuumMax.SetActive(true); //show the max level sign

                SoundManager.instance.PlayUpgSound(); //play the upgrade sound

                timesVacuumUpgraded++; //increment the counter for the vacuum upgrades
            }
            else //if the stat is already at max level
            {
                Debug.Log("Reached max vacuum already"); //print the warning in the console
            }
        }
        else //if the player does not have enough money
        {
            GameManager.instance.moneyAnimator.Play("MoneyNoBlob"); //play the no money animation
            SoundManager.instance.PlayNoMoneySound(); //play the no money sound
        }
    }

    public void UpgradeCapacity()
    {
        if (GameManager.moneyTotal >= capacityUpgradeCost) //if the player has enough money
        {
            if (timesCapacityUpgraded < maxUpgrades) //if the stat is not already at max level
            {
                GameManager.capacity += GameManager.instance.capacityUpg; //increase player's capacity by the set ammount

                Debug.Log("Capacity upgraded: " + GameManager.capacity); //print the current capacity number in the console

                StartCoroutine(ShowPopUp(capacityAnim.gameObject, capacityAnim)); //show the text popup

                BuyUpgrade(capacityUpgradeCost); //do the transaction

                capacityUpgradeCost += capacityCostChange; //increase the cost by the set ammount for next upgrade
                capacityCostNumber.text = capacityUpgradeCost.ToString(); //update the text of the cost

                if (timesCapacityUpgraded == (maxUpgrades - 1)) capacityMax.SetActive(true); //show the max level sign

                SoundManager.instance.PlayUpgSound(); //play the upgrade sound

                timesCapacityUpgraded++; //increment the counter for the capacity upgrades
            }
            else //if the stat is already at max level
            {
                Debug.Log("Reached max capacity already"); //print the warning in the console
            }
        }
        else //if the player does not have enough money
        {
            GameManager.instance.moneyAnimator.Play("MoneyNoBlob"); //play the no money animation
            SoundManager.instance.PlayNoMoneySound(); //play the no money sound
        }
    }

    public void UpdateShopUI()
    {
        moneyCount.text = GameManager.moneyTotal.ToString(); //update the money in UI

        //update the shop cost texts
        speedCostNumber.text = speedUpgradeCost.ToString();
        vacuumCostNumber.text = vacuumUpgradeCost.ToString();
        capacityCostNumber.text = capacityUpgradeCost.ToString();

        //show the max level text if a stat is at max level
        if (timesSpeedUpgraded >= maxUpgrades) speedMax.SetActive(true);
        if (timesVacuumUpgraded >= maxUpgrades) vacuumMax.SetActive(true);
        if (timesCapacityUpgraded >= maxUpgrades) capacityMax.SetActive(true);
    }

    public void BuyUpgrade(int cost)
    {
        GameManager.moneyTotal -= cost; //reduce the total money according to the cost of the upgrade
        moneyCount.text = GameManager.moneyTotal.ToString(); //update the money UI
    }

    IEnumerator ShowPopUp(GameObject textObject, Animator animator)
    {
        textObject.SetActive(true); //turn text object on
        animator.Play("UIPopup"); //play the animation

        yield return new WaitForSeconds(1f); //ayo hol up!

        textObject.SetActive(false); //turn the text object off
    }
}
