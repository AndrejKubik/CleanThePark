using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumColision : MonoBehaviour
{
    private SuckingMechanism trashType;

    private void OnTriggerEnter(Collider other)
    {
        trashType = other.GetComponent<SuckingMechanism>(); //check if the hitting object is trash

        if(trashType != null && GameManager.instance.canPull) //if it is a trash piece
        {
            if (!VacuumCollisionProximity.instance.trashNear) GameManager.instance.vacuum.SetActive(true); //if there no trash near then turn the vacuum collider back on
            GameManager.instance.collectedCount++; //increment the number of trash pieces collected
            Destroy(other.gameObject); //destroy the trash piece
            trashType = null; //reset the trash check
            //Debug.Log("Progress to next box: " + GameManager.instance.collectedCount);
        }
    }
}
