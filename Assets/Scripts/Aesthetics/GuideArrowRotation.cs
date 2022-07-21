using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrowRotation : MonoBehaviour
{
    private Vector3 target;

    private void Start()
    {
        target = GameManager.instance.deliveryIndicator.transform.position;
    }

    private void Update()
    {
        transform.LookAt(target);
    }
}
