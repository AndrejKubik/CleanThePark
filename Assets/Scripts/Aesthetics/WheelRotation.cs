using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.instance.isMoving && GameManager.instance.canMove) transform.Rotate(Vector3.forward * GameManager.wheelSpeed * GameManager.instance.wheelSpeedModifier * Time.deltaTime); //if the player is on the move spin his wheels boye!
    }
}
