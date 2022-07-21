using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    [SerializeField] private GameObject[] particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body") && GameManager.instance.stackCount > 0) //when the player enters the delivery indicator field
        {
            particles[GameManager.instance.stackCount - 1].SetActive(true); //show the proper particle according to the player's stack count

            GameManager.instance.DeliverStacks();

            GameManager.instance.StartDeliveryCooldown();
        }
    }
}
