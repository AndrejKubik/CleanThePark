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

    public static int speedUpgradeCost;
    public static int vacuumUpgradeCost;
    public static int capacityUpgradeCost ;

    public Animator speedAnim;
    public Animator vacuumAnim;
    public Animator capacityAnim;

    public GameObject speedMax;
    public GameObject vacuumMax;
    public GameObject capacityMax;

    private void Start()
    {
        UpdateShopUI();
        //if (GameManager.scenesLoaded <= 1)
        //{
        //    GameManager.scenesLoaded++;
        //}

        if (GameManager.scenesLoaded >= 2)
        {
            Time.timeScale = 0f; //freeze game time
            startMenu.SetActive(true);
        }
        else
        {
            joystick.SetActive(true); //activate the joystick
            menuButton.SetActive(true); //show the pause button
            Time.timeScale = 1f; //resume game time
            startMenu.SetActive(false); //hide the start menu
        }

        //if (GameManager.currentLevel == 1) ResetShop(); //on the first level reset shop
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

        AnalyticsManagerr.instance.LevelStart();
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
        AnalyticsManagerr.instance.LevelStart();

        SceneManager.LoadScene(0); //load the first level 
    }

    public void NextLevel()
    {
        GameManager.currentLevel++; //increment the level number
        GameManager.instance.SaveGame();
        AnalyticsManagerr.instance.LevelStart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //load the next level 
    }

    public void LoadLevel(int levelNumber)
    {
        Destroy(GameObject.FindGameObjectWithTag("Music")); //start the music from the start
        SceneManager.LoadScene(levelNumber);
    }

    public void UpgradeSpeed()
    {
        if (GameManager.moneyTotal >= speedUpgradeCost)
        {
            if (timesSpeedUpgraded < maxUpgrades)
            {
                GameManager.speed += GameManager.instance.speedUpg; //increase the player's speed by the chose value
                GameManager.wheelSpeed += GameManager.instance.wheelSpeedUpg; //rotate wheels faster
                StartCoroutine(ShowPopUp(speedAnim.gameObject, speedAnim));
                Debug.Log("Speed upgraded: " + GameManager.speed);

                BuyUpgrade(speedUpgradeCost); //do the transaction

                speedUpgradeCost += speedCostChange; //increase the cost by the set ammount for next upgrade
                speedCostNumber.text = speedUpgradeCost.ToString(); //update the text of the cost

                if (timesSpeedUpgraded == (maxUpgrades - 1)) speedMax.SetActive(true); //show the max level sign

                SoundManager.instance.PlayUpgSound();

                timesSpeedUpgraded++; //increment the counter for speed upgrades
            }
            else
            {
                Debug.Log("Reached max speed already");
            }
        }
        else
        {
            GameManager.instance.moneyAnimator.Play("MoneyNoBlob");
            SoundManager.instance.PlayNoMoneySound();
        }
    }

    public void UpgradeVacuum()
    {
        if (GameManager.moneyTotal >= vacuumUpgradeCost)
        {
            if (timesVacuumUpgraded < maxUpgrades)
            {
                GameManager.instance.TurnVacuumOff(); //hide the current vacuum indicator
                GameManager.vacuumWidth++; //change the vacuum model to the next level model
                GameManager.pullSpeed += GameManager.instance.pullUpg; //increase the pulling strength
                StartCoroutine(ShowPopUp(vacuumAnim.gameObject, vacuumAnim));
                Debug.Log("current vacuum: " + GameManager.vacuumWidth);
                if (GameManager.instance.canPull) GameManager.instance.TurnVacuumOn(); //if the player doesn't have max stacks on his back, show the next level vacuum indicator

                BuyUpgrade(vacuumUpgradeCost); //do the transaction

                vacuumUpgradeCost += vacuumCostChange; //increase the cost by the set ammount for next upgrade
                vacuumCostNumber.text = vacuumUpgradeCost.ToString(); //update the text of the cost

                if (timesVacuumUpgraded == (maxUpgrades - 1)) vacuumMax.SetActive(true); //show the max level sign

                SoundManager.instance.PlayUpgSound();

                timesVacuumUpgraded++; //increment the counter for the vacuum upgrades
            }
            else
            {
                Debug.Log("Reached max vacuum already");
            }
        }
        else
        {
            GameManager.instance.moneyAnimator.Play("MoneyNoBlob");
            SoundManager.instance.PlayNoMoneySound();
        }
    }

    public void UpgradeCapacity()
    {
        if (GameManager.moneyTotal >= capacityUpgradeCost)
        {
            if (timesCapacityUpgraded < maxUpgrades)
            {
                GameManager.capacity += GameManager.instance.capacityUpg; //increase player's capacity by the set ammount
                Debug.Log("Capacity upgraded: " + GameManager.capacity);
                StartCoroutine(ShowPopUp(capacityAnim.gameObject, capacityAnim));

                BuyUpgrade(capacityUpgradeCost); //do the transaction

                capacityUpgradeCost += capacityCostChange; //increase the cost by the set ammount for next upgrade
                capacityCostNumber.text = capacityUpgradeCost.ToString(); //update the text of the cost

                if (timesCapacityUpgraded == (maxUpgrades - 1)) capacityMax.SetActive(true); //show the max level sign

                SoundManager.instance.PlayUpgSound();

                timesCapacityUpgraded++; //increment the counter for the capacity upgrades
            }
            else
            {
                Debug.Log("Reached max capacity already");
            }
        }
        else
        {
            GameManager.instance.moneyAnimator.Play("MoneyNoBlob");
            SoundManager.instance.PlayNoMoneySound();
        }
    }

    public void UpdateShopUI()
    {
        moneyCount.text = GameManager.moneyTotal.ToString(); //update the money in UI

        speedCostNumber.text = speedUpgradeCost.ToString();
        vacuumCostNumber.text = vacuumUpgradeCost.ToString();
        capacityCostNumber.text = capacityUpgradeCost.ToString();

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
