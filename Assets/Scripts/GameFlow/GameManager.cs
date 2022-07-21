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
        Application.targetFrameRate = 60;

        LoadGame();
        if(scenesLoaded <= 1) scenesLoaded++;
        Debug.Log(scenesLoaded);

        collectedCount = 0;
        stackCount = 0;
        canPull = true;
        destroyCounter = 0;
        levelClear = false;
        canMove = true;

        Debug.Log("Current Level: " + currentLevel);
        Debug.Log("wheel speed: " + wheelSpeed);
        Debug.Log("Speed: " + speed);
        Debug.Log("Capacity: " + maxBoxesToCarry);
        Debug.Log("Vacuum: " + vacuumWidth);
        Debug.Log("Money: " + moneyTotal);

        TurnVacuumOn();
    }

    private void Update()
    {
        if (collectedCount >= numberOfTrashToCollect) SpawnStackObject();

        if(stackCount >= capacity && !maxCapacityReached) //if player reaches max stacks
        {
            SoundManager.instance.PlayFullCapSound();
            MaxCapacityPopup(); //show max cap popup
            TurnVacuumOff(); //hide the vacuum to stop the player from taking in more garbage
            pullTarget.GetComponent<BoxCollider>().enabled = false; //disable the vacuum's collider
            canPull = false; //disable further pulling of garbage
            guideArrow.SetActive(true); //show the guide arrow for dumping spot
            maxCapacityReached = true;
        }

        if (destroyCounter >= garbageCount)
        {
            if (stackCount < 1 && !levelFinished) SpawnStackObject();

            UIController.instance.winText.SetActive(true); //show the victory message
            levelClear = true;
            if(levelFinished) guideArrow.SetActive(false); //hide the guide arrow
            else
            {
                guideArrow.SetActive(true); //show the guide arrow
                TurnVacuumOff(); //hide the vacuum 
            }

            if (!soundPlayed)
            {
                SoundManager.instance.PlayLevelClearSound();
                soundPlayed = true;
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
        TurnVacuumOff();
        for(int i = stackCount - 1; i >= 0; i--) //for every active player stack
        {
            stacks[i].SetActive(false); //turn off the current box
            moneyTotal += moneyGain; //transmute garbage to money
            moneyAnimator.Play("MoneyBlob"); //play the money animation
            UIController.instance.moneyCount.text = moneyTotal.ToString(); //update the money on UI
            //Debug.Log("Total Cash: " + moneyTotal); //tell me how much i got
            yield return new WaitForSeconds(delay); //ayo hol' up
        }
        canMove = true; //let the player move again
        TurnVacuumOn();
        maxCapacityReached = false;
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
        StartCoroutine(DeliveryCooldown(deliveryCooldown, deliveryIndicator));
    }

    public void TurnVacuumOn()
    {
        vacuums[vacuumWidth].SetActive(true); //show the current level vacuum
        indicators[vacuumWidth].SetActive(true); //show the current level vacuum indicator
        vacuumParticles.SetActive(true); //play the poof
        if (!levelClear) vacuumParticles.SetActive(true); //play the poof
    }

    public void TurnVacuumOff()
    {
        vacuums[vacuumWidth].SetActive(false); //hide the current level vacuum
        indicators[vacuumWidth].SetActive(false); //hide the current level vacuum indicator
        if (!levelClear) vacuumParticles.SetActive(true); //play the poof
    }
    public void MaxCapacityPopup() { Instantiate(capFull, popupPosition.position, transform.rotation); }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        SaveGameData data = SaveSystem.LoadGame();

        if(data != null)
        {
            if (currentLevel == 0) currentLevel = 1;
            else currentLevel = data.currentLevel;

            moneyTotal = data.moneyTotal;
            wheelSpeed = data.wheelSpeed;
            speed = data.speed;
            capacity = data.capacity;
            vacuumWidth = data.vacuumWidth;
            pullSpeed = data.pullSpeed;

            if(scenesLoaded <= 1) UIController.instance.LoadLevel(currentLevel - 1);

        }
        else
        {
            ResetGame();
            UIController.instance.LoadLevel(0); //load the first level
        }
        
    }

    public void ResetGame()
    {
        currentLevel = 1;
        speed = playerMoveSpeed; //set the player speed 
        capacity = maxBoxesToCarry; //set the player capacity to the set starting value
        vacuumWidth = 0; //set the vacuum level to 0
        pullSpeed = startPullSpeed; //reset the pull speed of the vacuum
        wheelSpeed = wheelSpeedRaw; //reset the wheel rotation speed to the default value
        moneyTotal = 0; //reset the money
        UIController.instance.moneyCount.text = moneyTotal.ToString(); //update the money ui

        UIController.instance.ResetShop(); //reset shop costs

        Debug.Log("Game save file reset!");

        SaveGame();
    }
}
