using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f); //destroy this object after 1 second
    }

    private void Update()
    {
        transform.Translate(Vector3.up * 2f * Time.deltaTime); //move this object upwards
    }
}
