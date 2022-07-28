using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header(">> Drag-in used Joystick << ")]
    [SerializeField] private Joystick joystick;

    [Header(">> Drag-in player's Rigidbody Component << ")]
    [SerializeField] private Rigidbody rigidbody;

    [Header(">> Set Movement Values << ")]
    [SerializeField] private float stopTime;

    [Header(">> Set Rotation Speed << ")]
    [SerializeField] private float rotationSpeed;

    [Header(">> Joystick Values Debug(Don't touch) << ")]
    [SerializeField] private float inputX;
    [SerializeField] private float inputY;

    private void FixedUpdate()
    {
        //store joystick input values 
        inputX = joystick.Horizontal;
        inputY = joystick.Vertical;

        if ((inputX != 0 || inputY != 0) && !GameManager.instance.levelFinished) //when the joystick is not in neutral position
        {
            RotatePlayer(); //rotate the player relative to the joystick position
            if(GameManager.instance.canMove) MoveForward(GameManager.speed); //move the player forward over time by the chosen speed
        }
        else StopMoving(stopTime); //stop further movement
    }

    void RotatePlayer()
    {
        Vector3 lookDirection = new Vector3(inputX, 0, inputY); //calculate the direction vector using joystick input values
        if(lookDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up); //calculate the target quaternion value to rotate towards

            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime); //actually rotate the player towards the calculated direction
        }
    }

    void MoveForward(float rawSpeed)
    {
        rigidbody.velocity = new Vector3(joystick.Direction.x * rawSpeed, rigidbody.velocity.y, joystick.Direction.y * rawSpeed) * Time.deltaTime; //set the velocity of moving forward over time

        if (Mathf.Abs(joystick.Direction.x) > Mathf.Abs(joystick.Direction.y)) GameManager.instance.wheelSpeedModifier = Mathf.Abs(joystick.Direction.x);
        else GameManager.instance.wheelSpeedModifier = Mathf.Abs(joystick.Direction.y);

        if (!GameManager.instance.isMoving) GameManager.instance.isMoving = true; //change the movement state if not changed already
    }

    void StopMoving(float stopTime)
    {
        rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, Vector3.zero, stopTime * Time.deltaTime); //change the velocity vector for the object to slow down 
        if (GameManager.instance.isMoving) GameManager.instance.isMoving = false; //change the movement state if not changed already
    }
}
