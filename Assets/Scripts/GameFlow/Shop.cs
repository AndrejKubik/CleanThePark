using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private bool playerInShop;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Body") && !playerInShop)
        {
            playerInShop = true; //prevent the shop from opening again while inside of the shop trigger
            UIAnimations.instance.OpenShop(); //if the player enters the shop field, slide the shop in
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Body")) //if the shop wasn't already close with the X button
        {
            playerInShop = false; //let the player open the shop once again
        }
    }
}
