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

    [SerializeField] private AnimationClip shopOpen;
    [SerializeField] private AnimationClip shopClose;

    public void OpenShop()
    {
        bool hasPlayed = false;

        if(!hasPlayed)
        {
            animator.Play(shopOpen.name);
            hasPlayed = true;
        }
    }

    public void CloseShop()
    {
        bool hasPlayed = false;

        if (!hasPlayed)
        {
            animator.Play(shopClose.name);
            hasPlayed = true;
        }
    }
}
