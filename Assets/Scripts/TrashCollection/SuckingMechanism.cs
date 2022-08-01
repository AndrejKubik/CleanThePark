using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckingMechanism : MonoBehaviour
{
    [SerializeField] private bool isPulling;
    private bool isGrounded;
    private Rigidbody rb;

    [SerializeField] private GameObject indicatorParticle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //store the object's rigidbody component
        isGrounded = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Vacuum") && GameManager.instance.canPull) //if the object is hit by the vacuum and if the player is not carrying max number of boxes on his back
        {
            isPulling = true; //trigger the pulling state
            SoundManager.instance.PlayVacuumSound(); //play the suction sound
        }
        else if(other.CompareTag("Proximity") && !GameManager.instance.canPull) //if the garbage piece somehow touches the vacuum from the sides
        {
            ActivateGravity(rb); //let it fall to the ground
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vacuum")) //if the object leaves the vacuum indicator
        {
            isPulling = false; //stop pulling the object
            isGrounded = false;
            ActivateGravity(rb); //activate gravity usage for the dropped piece of trash
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isGrounded) //if the trash piece is on the ground
        {
            DeactivateGravity(rb); //remove the gravity from the dropped piece of trash
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.destroyCounter++; //increment the destroy counter when this piece is removed from the game
    }

    private void Update()
    {
        if(isPulling) PullTrash(); //if the pulling has been triggered start pulling the trash

        if (!GameManager.instance.canPull) isPulling = false; //if the pulling is disabled, stop pulling garbage

        if ((GameManager.instance.garbageCount - GameManager.instance.destroyCounter) <= 5) indicatorParticle.SetActive(true);
    }

    void PullTrash()
    {
        //move the trash object towards the vacuum trigger
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.pullTarget.position, GameManager.pullSpeed * Time.deltaTime);
    }

    void ActivateGravity(Rigidbody rigidbody)
    {
        rigidbody.isKinematic = false;
    }

    void DeactivateGravity(Rigidbody rigidbody)
    {
        rigidbody.isKinematic = true;
    }
}
