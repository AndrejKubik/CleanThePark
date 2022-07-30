using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static bool closeButtonClicked;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Body")) UIAnimations.instance.OpenShop(); //if the player enters the shop field, slide the shop in
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Body") && !closeButtonClicked) //if the shop wasn't already close with the X button
    //    {
    //        UIAnimations.instance.CloseShop(); //if the player leaves the shop field, slide the shop out
    //        closeButtonClicked = false; //reset the X button check
    //    }
    //}
}
