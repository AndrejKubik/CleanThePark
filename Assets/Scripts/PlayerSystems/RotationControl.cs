using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControl : MonoBehaviour
{
    [SerializeField] private GameObject capsuleCollider;
    [SerializeField] private GameObject boxCollider;
    private bool isPassing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && !capsuleCollider.activeSelf)
        {
            capsuleCollider.SetActive(true);
            boxCollider.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall") && capsuleCollider.activeSelf)
        {
            StartCoroutine(ActivateBox());
        }
    }

    IEnumerator ActivateBox()
    {
        yield return new WaitForSeconds(1f);

        boxCollider.SetActive(true);
        capsuleCollider.SetActive(false);
    }
}
