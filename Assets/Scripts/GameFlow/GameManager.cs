using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    [Header(">>>>>>>>>>>>>>>| VACUUM VALUES |<<<<<<<<<<<<<<<")]

    public Transform pullTarget;
    public GameObject vacuum;
    public bool canPull;

    [Header(">>>>>>>>>>>>>>>| STACK OBJECTS AND STACK COUNTERS |<<<<<<<<<<<<<<<")]

    public List<GameObject> stacks;
    public int collectedCount;
    public int stackCount;

    [Header(">>>>>>>>>>>>>>>| MONEY VALUES |<<<<<<<<<<<<<<<")]

    public static int moneyTotal = 0;
    public static int moneyGain;

    [Header(">>>>>>>>>>>>>>>| AESTHETHIC THINGS |<<<<<<<<<<<<<<<")]

    private float particleDelay = 0.05f;
    public float deliveryCooldown;

    public GameObject deliveryIndicator;
    public GameObject guideArrow;
    public Animator moneyAnimator;
    public Animator dumpsterAnimator;
    public GameObject deliveryParticles;
    public GameObject vacuumParticles;

    private bool soundPlayed;
    private bool maxCapacityReached;

    [Header(">>>>>>>>>>>>>>>| TEXT POPUP DATA |<<<<<<<<<<<<<<<")]

    public Transform popupPosition;
    [SerializeField] private GameObject capFull;

    [Header(">>>>>>>>>>>>>>>| MOVEMENT VALUES |<<<<<<<<<<<<<<<")]

    public bool canMove;
    public bool isMoving;

    public float wheelSpeedModifier;
    public static float wheelSpeed;

    [Header(">>>>>>>>>>>>>>>| LEVEL CONTROL VALUES |<<<<<<<<<<<<<<<")]

    public static int currentLevel;

    public int garbageCount;
    public int destroyCounter;
    public bool levelClear;
    public bool levelFinished;
    public bool levelStarted;

    public List<Level> levels;
    public Level currentLevelData;
    public Transform playAreaParent;
    public Transform garbageParent;
    public Transform eyeCandyParent;
    public Transform environmentParent;

    [Header(">>>>>>>>>>>>>>>| UPGRADE EFFECTIVE VALUES |<<<<<<<<<<<<<<<")]

    public AestheticValuesControl aesthethicsData;
    public UpgradeValuesControl upgradeChangesData;

    public float speedUpg = 400f;
    public int capacityUpg = 3;

    //CURRENT PLAYER STATS

    public static int speedLevel = 0;
    public static float speed;
    public static int capacity;
    public static int vacuumLevel;
    public static float pullSpeed;

    [Header(">>>>>>>>>>>>>>>| LIST OF ALL PLAYER UPGRADE MODELS |<<<<<<<<<<<<<<<")]

    public List<GameObject> vacuums;
    public List<GameObject> wheels;

    public static int scenesLoaded = 0;

    private void Start()
    {
        Application.targetFrameRate = 60; //make the game try it's best to get at least 60 fps

        LoadGame(); //load in the game data

        currentLevelData = levels[currentLevel - 1]; //load in the data from the current level from the list

        TurnWheelsOn(); //show the wheel models according to the upgrade level

        GenerateLevelChunk(currentLevelData.playAreaChunkPrefab, playAreaParent); //spawn the play area chunk according to the current level data

        //reset the game counters
        collectedCount = 0;
        stackCount = 0;
        destroyCounter = 0;
        moneyGain = currentLevelData.moneyGain;

        //reset tha game states
        canPull = true;
        levelClear = false;
        canMove = true;

        //print the game data values in the console
        Debug.Log("Level name: " + currentLevelData.levelName);
        Debug.Log("Current Level: " + currentLevel);
        Debug.Log("wheel speed: " + wheelSpeed);
        Debug.Log("Speed: " + speed);
        Debug.Log("Capacity: " + capacity);
        Debug.Log("Vacuum: " + vacuumLevel);
        Debug.Log("Money: " + moneyTotal);
        Debug.Log("Money Gain on this level: " + moneyGain);

        TurnVacuumOn(); //activate the player's vacuum
    }

    private void Update()
    {
        if (collectedCount >= aesthethicsData.numberOfTrashForStack) SpawnStackObject(); //give player a stack on his back if he has collected enough garbage

        if(stackCount >= capacity && !maxCapacityReached) //if player reaches max stacks
        {
            if(!levelClear)
            {
                SoundManager.instance.PlayFullCapSound(); //play the full capacity sound
                MaxCapacityPopup(); //show max cap text popup
            }

            TurnVacuumOff(); //hide the vacuum to stop the player from taking in more garbage
            pullTarget.GetComponent<BoxCollider>().enabled = false; //disable the vacuum's collider
            canPull = false; //disable further pulling of garbage
            guideArrow.SetActive(true); //show the guide arrow for dumping spot
            maxCapacityReached = true; //change the capacity state to prevent looping of the above
        }

        if (destroyCounter >= garbageCount && levelStarted) //if the player has collected more
        {
            if (stackCount < 1 && !levelFinished) SpawnStackObject(); //if somehow player collects all of the garbage on the level and does not have a box on his back, give him one on the house!

            UIController.instance.winText.SetActive(true); //show the victory message
            levelClear = true; //change the game state

            if(levelFinished) guideArrow.SetActive(false); //hide the guide arrow
            else //if the level is not finished yet
            {
                guideArrow.SetActive(true); //show the guide arrow
                TurnVacuumOff(); //hide the vacuum 
            }

            if (!soundPlayed) //if the level clear sound has not been played yet
            {
                SoundManager.instance.PlayLevelClearSound(); //play it
                soundPlayed = true; //and prevent looping of the sound
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) moneyTotal += 1000; //FAKIN CHEATOR
    }

    public void SpawnStackObject()
    {
        stacks[stackCount].SetActive(true); //show stack object on the first empty slot
        stackCount++; //increment the active stacks number
        collectedCount = 0; //reset the production progress
    }

    IEnumerator StackRemoval(float delay)
    {
        canMove = false; //stop the player from moving 
        TurnVacuumOff(); //deactivate the player's vacuum
        for(int i = stackCount - 1; i >= 0; i--) //for every active player stack
        {
            stacks[i].SetActive(false); //turn off the current box
            moneyTotal += moneyGain; //transmute garbage to money
            moneyAnimator.Play("MoneyBlob"); //play the money animation
            UIController.instance.moneyCount.text = moneyTotal.ToString(); //update the money on UI
            yield return new WaitForSeconds(delay); //ayo hol' up
        }
        canMove = true; //let the player move again
        TurnVacuumOn(); //activate the player's vacuum
        maxCapacityReached = false; //let the player collect garbage again
    }

    public void DeliverStacks()
    {
        guideArrow.SetActive(false); //hide the guide arrow
        TurnVacuumOn(); //allow the player to use the vacuum again by showing it
        pullTarget.GetComponent<BoxCollider>().enabled = true; //re-enable the vacuum's collider

        dumpsterAnimator.Play("DoorsOpen"); //play the dumpster opening animation
        SoundManager.instance.PlayDumpsterSound(); //play the opening sound
        StartCoroutine(StackRemoval(particleDelay)); //start hiding the stacks one by one
        stackCount = 0; //reset the current stack counter

        canPull = true; //enable garbage pulling again
    }

    IEnumerator DeliveryCooldown(float delay, GameObject deliveryIndicator)
    {
        deliveryIndicator.SetActive(false); //hide the drop-off indicator
        yield return new WaitForSeconds(delay); //ayo hol' up

        if(!levelClear) deliveryIndicator.SetActive(true); //show the drop-off indicator
    }

    public void StartDeliveryCooldown()
    {
        StartCoroutine(DeliveryCooldown(deliveryCooldown, deliveryIndicator));  //hide the delivery field and reactivate it after a while
    }

    public void TurnVacuumOn()
    {
        vacuums[vacuumLevel].SetActive(true); //show the current level vacuum
        if (!levelClear) vacuumParticles.SetActive(true); //play the poof  
    }

    public void TurnVacuumOff()
    {
        vacuums[vacuumLevel].SetActive(false); //hide the current level vacuum
        if (!levelClear) vacuumParticles.SetActive(true); //if the level is not yet clear play the poof
    }

    public void TurnWheelsOn()
    {
        wheels[speedLevel].SetActive(true); //show current speed models
    }

    public void UpgradeWheels()
    {
        wheels[speedLevel].SetActive(false); //hide the current speed models
        speedLevel++; //increment the speed level number
        wheels[speedLevel].SetActive(true); //show new speed models
    }

    public void MaxCapacityPopup() { Instantiate(capFull, popupPosition.position, transform.rotation); }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        SaveGameData data = SaveSystem.LoadGame();

        if(data != null) //if a save file exists
        {
            //change the game flow and stat values to the save file's values
            currentLevel = data.currentLevel;
            moneyTotal = data.moneyTotal;

            speedLevel = data.speedLevel;
            wheelSpeed = data.wheelSpeed;
            speed = data.speed;
            capacity = data.capacity;
            vacuumLevel = data.vacuumLevel;
            pullSpeed = data.pullSpeed;

            //change the shop costs to the values from the save file
            UIController.speedUpgradeCost = data.speedUpgCost;
            UIController.vacuumUpgradeCost = data.vacuumUpgCost;
            UIController.capacityUpgradeCost = data.capacityUpgCost;

            //remeber how many times every upgrade has been bought
            UIController.speedUpgCount = data.speedUpgCount;
            UIController.vacuumUpgCount = data.vacuumUpgCount;
            UIController.capacityUpgCount = data.capacityUpgCount;
        }
        else //if there is no save file yet
        {
            ResetGame(); //reset all the game values to default
        }
    }

    public void ResetGame()
    {
        currentLevel = 1; //get the level number to level 1
        speed = aesthethicsData.startMovementSpeed; //set the player speed 
        capacity = aesthethicsData.startCapacity; //set the player capacity to the set starting value
        vacuumLevel = 0; //set the vacuum level to 0
        pullSpeed = aesthethicsData.vacuumStrengthStart; //reset the pull speed of the vacuum
        wheelSpeed = aesthethicsData.wheelSpeedStart; //reset the wheel rotation speed to the default value
        moneyTotal = 0; //reset the money

        UIController.instance.moneyCount.text = moneyTotal.ToString(); //update the money ui
        UIController.instance.ResetShop(); //reset shop costs

        Debug.Log("Game save file reset!"); //print the notification in the console

        SaveGame(); //save the game data just in case
    }

    private void GenerateLevelChunk(GameObject playAreaChunk, Transform positionParentObject)
    {
        Instantiate(playAreaChunk, positionParentObject); //spawn a level chunk as a child object of the position slot parent
    }

    public void UpgradeSpeedValues()
    {
        speed += upgradeChangesData.movementSpeedChange; //increase the player's speed by the chose value
        wheelSpeed += aesthethicsData.wheelSpeedChange; //rotate wheels faster

        Debug.Log("Speed upgraded: " + speed); //print current speed in the console
    }

    public void UpgradeVacuumValues()
    {
        vacuumLevel++; //change the vacuum model to the next level model
        pullSpeed += aesthethicsData.vacuumStrengthChange; //increase the pulling strength

        Debug.Log("current vacuum: " + vacuumLevel); //print the current vacuum number in the console
    }

    public void UpgradeCapacityValues()
    {
        capacity += upgradeChangesData.capacityChange; //increase player's capacity by the set ammount

        Debug.Log("Capacity upgraded: " + capacity); //print the current capacity number in the console
    }
}
