using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCollisionProximity : MonoBehaviour
{
    #region Singleton
    public static VacuumCollisionProximity instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public List<GameObject> trash;
    public bool trashNear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Garbage")) //if a trash piece comes in the vacuum side safeguard area
        {
            trash.Add(other.gameObject); //add the current trash to the list
            trashNear = true; //trigger the proximity check ???????
            GameManager.instance.vacuum.SetActive(false); //turn off the player's vacuum object
        }
    }

    private void Update()
    {
        if (trashNear) CheckProximity(); //if there is trash on the sides check again
    }

    void CheckProximity()
    {
        int counter = 0;

        for(int i = 0; i < trash.Count; i++)
        {
            if (trash[i] != null && !trashNear) counter++; 
        }

        if (counter > 0)
        {
            Debug.Log("there is trash on the sides");

            trashNear = true;
        }
        else
        {
            //Debug.Log("there is no more trash on the sides");
            
            trashNear = false;
        }
    }
}
