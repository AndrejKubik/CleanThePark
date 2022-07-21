using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public AudioSource audio;

    public AudioClip vacuumSound;
    public AudioClip deliverySound;
    public AudioClip dumpsterSound;

    public AudioClip levelClearSound;
    public AudioClip levelEndSound;

    public AudioClip noMoneySound;
    public AudioClip fullCapSound;
    public AudioClip upgradeSound;

    public void PlayVacuumSound() { audio.PlayOneShot(vacuumSound, 0.8f); }
    public void PlayDeliverySound() { audio.PlayOneShot(deliverySound, 2f); }
    public void PlayDumpsterSound() { audio.PlayOneShot(dumpsterSound, 1.5f); }
    public void PlayLevelClearSound() { audio.PlayOneShot(levelClearSound, 3f); }
    public void PlayLevelEndSound() { audio.PlayOneShot(levelEndSound, 3f); }
    public void PlayNoMoneySound() { audio.PlayOneShot(noMoneySound, 2f); }
    public void PlayFullCapSound() { audio.PlayOneShot(fullCapSound, 1.5f); }
    public void PlayUpgSound() { audio.PlayOneShot(upgradeSound, 1.5f); }
}
