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

    public GameObject bgMusic;

    public AudioClip vacuumSound;
    public float vacuumSoundVolume = 0.8f;

    public AudioClip deliverySound;
    public float deliverySoundVolume = 2f;

    public AudioClip dumpsterSound;
    public float dumpsterSoundVolume = 1.5f;

    public AudioClip levelClearSound;
    public float levelClearSoundVolume = 3f;

    public AudioClip levelEndSound;
    public float levelEndSoundVolume = 3f;

    public AudioClip noMoneySound;
    public float noMoneySoundVolume = 2f;

    public AudioClip fullCapSound;
    public float fullCapSoundVolume = 1.5f;

    public AudioClip upgradeSound;
    public float upgradeSoundVolume = 1.5f;

    private void Start()
    {
        if (GameManager.scenesLoaded == 0) Instantiate(bgMusic, Vector3.zero, transform.rotation);
    }

    public void PlayVacuumSound() { audio.PlayOneShot(vacuumSound, vacuumSoundVolume); }
    public void PlayDeliverySound() { audio.PlayOneShot(deliverySound, deliverySoundVolume); }
    public void PlayDumpsterSound() { audio.PlayOneShot(dumpsterSound, dumpsterSoundVolume); }
    public void PlayLevelClearSound() { audio.PlayOneShot(levelClearSound, levelClearSoundVolume); }
    public void PlayLevelEndSound() { audio.PlayOneShot(levelEndSound, levelEndSoundVolume); }
    public void PlayNoMoneySound() { audio.PlayOneShot(noMoneySound, noMoneySoundVolume); }
    public void PlayFullCapSound() { audio.PlayOneShot(fullCapSound, fullCapSoundVolume); }
    public void PlayUpgSound() { audio.PlayOneShot(upgradeSound, upgradeSoundVolume); }
}
