using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    #region Singleton

    public static UIAnimations instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] Animator animator;
    [SerializeField] Animator camera;

    [SerializeField] private AnimationClip shopOpen;
    [SerializeField] private AnimationClip shopClose;

    public void OpenShop()
    {
        bool hasPlayed = false;

        if(!hasPlayed)
        {
            StartCoroutine(ActivateShop(0.6f));

            hasPlayed = true;
        }
    }

    public void CloseShop()
    {
        bool hasPlayed = false;

        if (!hasPlayed)
        {
            StartCoroutine(DeactivateShop(0.6f));

            hasPlayed = true;
        }
    }

    IEnumerator ActivateShop(float delay)
    {
        camera.SetTrigger("OpenShop"); //zoom in on the player

        yield return new WaitForSeconds(delay);

        GameManager.instance.canMove = false;
        UIController.instance.joystick.SetActive(false); //disable the player movement

        yield return new WaitForSeconds(0.833f - delay);

        animator.Play(shopOpen.name); //slide the shop panel upwards
    }

    IEnumerator DeactivateShop(float delay)
    {
        animator.Play(shopClose.name); //slide the shop panel downwards
        camera.SetTrigger("CloseShop"); //zoom out from the player

        yield return new WaitForSeconds(delay);

        GameManager.instance.canMove = true;
        UIController.instance.joystick.SetActive(true); //enable the player movement
    }
}
