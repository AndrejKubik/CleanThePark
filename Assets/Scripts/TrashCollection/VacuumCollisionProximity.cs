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
        if (other.CompareTag("Garbage"))
        {
            trash.Add(other.gameObject); //add the current trash to the list
            GameManager.instance.vacuum.SetActive(false);
        }
    }

    private void Update()
    {
        if (trashNear) CheckProximity();
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
            Debug.Log("kurac2");

            trashNear = true;
        }
        else
        {
            Debug.Log("kurac");
            
            trashNear = false;
        }
    }
}
