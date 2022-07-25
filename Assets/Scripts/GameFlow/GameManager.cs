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

    public float startPullSpeed;
    public Transform pullTarget;
    public GameObject vacuum;
    public bool canPull;

    [Header(">>>>>>>>>>>>>>>| STACK OBJECTS AND STACK COUNTERS |<<<<<<<<<<<<<<<")]

    public List<GameObject> stacks;
    public int collectedCount;
    public int stackCount;
    public int numberOfTrashToCollect;
    public int maxBoxesToCarry = 4;

    [Header(">>>>>>>>>>>>>>>| MONEY VALUES |<<<<<<<<<<<<<<<")]

    public static int moneyTotal = 0;
    public int moneyGain;

    [Header(">>>>>>>>>>>>>>>| AESTHETHIC THINGS |<<<<<<<<<<<<<<<")]

    public float particleDelay = 0.05f;
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

    public float wheelSpeedRaw;
    public float wheelSpeedModifier;
    public static float wheelSpeed = 500f;

    public float playerMoveSpeed;

    [Header(">>>>>>>>>>>>>>>| LEVEL CONTROL VALUES |<<<<<<<<<<<<<<<")]

    public static int currentLevel;

    public int garbageCount;
    public int destroyCounter;
    public bool levelClear;
    public bool levelFinished;
    public bool levelStarted;
    public List<Level> levels;
    public Level currentLevelData;

    [Header(">>>>>>>>>>>>>>>| UPGRADE EFFECTIVE VALUES |<<<<<<<<<<<<<<<")]

    public float speedUpg = 400f;
    public int capacityUpg = 3;
    public float pullUpg = 5;
    public float wheelSpeedUpg = 500f;

    //CURRENT PLAYER STATS

    public static float speed = 1800f;
    public static int capacity;
    public static int vacuumWidth = 0;
    public static float pullSpeed = 13f;

    [Header(">>>>>>>>>>>>>>>| LIST OF ALL PLAYER VACUUM MODELS |<<<<<<<<<<<<<<<")]

    public List<GameObject> vacuums;
    public List<GameObject> indicators;

    public static int scenesLoaded = 0;

    private void Start()
    {
        Application.targetFrameRate = 60; //make the game try it's best to get at least 60 fps

        LoadGame(); //load in the game data

        currentLevelData = levels[currentLevel - 1]; //load in the data from the current level from the list

        //reset the game counters
        collectedCount = 0;
        stackCount = 0;
        destroyCounter = 0;

        //reset tha game states
        canPull = true;
        levelClear = false;
        canMove = true;

        //print the game data values in the console
        Debug.Log("Level name: " + currentLevelData.levelName);
        Debug.Log("Current Level: " + currentLevel);
        Debug.Log("wheel speed: " + wheelSpeed);
        Debug.Log("Speed: " + speed);
        Debug.Log("Capacity: " + maxBoxesToCarry);
        Debug.Log("Vacuum: " + vacuumWidth);
        Debug.Log("Money: " + moneyTotal);

        TurnVacuumOn(); //activate the player's vacuum
    }

    private void Update()
    {
        if (collectedCount >= numberOfTrashToCollect) SpawnStackObject(); //give player a stack on his back if he has collected enough garbage

        if(stackCount >= capacity && !maxCapacityReached) //if player reaches max stacks
        {
            SoundManager.instance.PlayFullCapSound(); //play the full capacity sound
            MaxCapacityPopup(); //show max cap text popup
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
        vacuums[vacuumWidth].SetActive(true); //show the current level vacuum
        indicators[vacuumWidth].SetActive(true); //show the current level vacuum indicator
        vacuumParticles.SetActive(true); //play the poof
        //if (!levelClear) vacuumParticles.SetActive(true); //play the poof  ???
    }

    public void TurnVacuumOff()
    {
        vacuums[vacuumWidth].SetActive(false); //hide the current level vacuum
        indicators[vacuumWidth].SetActive(false); //hide the current level vacuum indicator
        if (!levelClear) vacuumParticles.SetActive(true); //if the level is not yet clear play the poof
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
            //if (currentLevel == 0) currentLevel = 1;
            //else currentLevel = data.currentLevel;

            //change the game flow and stat values to the save file's values
            currentLevel = data.currentLevel;
            moneyTotal = data.moneyTotal;
            wheelSpeed = data.wheelSpeed;
            speed = data.speed;
            capacity = data.capacity;
            vacuumWidth = data.vacuumWidth;
            pullSpeed = data.pullSpeed;
        }
        else //if there is no save file yet
        {
            ResetGame(); //reset all the game values to default
        }
        
    }

    public void ResetGame()
    {
        currentLevel = 1; //get the level number to level 1
        speed = playerMoveSpeed; //set the player speed 
        capacity = maxBoxesToCarry; //set the player capacity to the set starting value
        vacuumWidth = 0; //set the vacuum level to 0
        pullSpeed = startPullSpeed; //reset the pull speed of the vacuum
        wheelSpeed = wheelSpeedRaw; //reset the wheel rotation speed to the default value
        moneyTotal = 0; //reset the money

        UIController.instance.moneyCount.text = moneyTotal.ToString(); //update the money ui
        UIController.instance.ResetShop(); //reset shop costs

        Debug.Log("Game save file reset!"); //print the notification in the console

        SaveGame(); //save the game data just in case
    }
}
