using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Body")) UIAnimations.instance.OpenShop();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Body")) UIAnimations.instance.CloseShop();
    }
}
