using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrowRotation : MonoBehaviour
{
    private Vector3 target;

    private void Start()
    {
        target = GameManager.instance.deliveryIndicator.transform.position; //get the target reference from the game manager
    }

    private void Update()
    {
        transform.LookAt(target); //turn towards the target
    }
}
