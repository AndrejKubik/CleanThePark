using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.instance.isMoving && GameManager.instance.canMove) //if the player is on the move
        {
            transform.Rotate(Vector3.forward * GameManager.wheelSpeed * GameManager.instance.wheelSpeedModifier * Time.deltaTime); //spin his wheels boye!
        }
    }
}
