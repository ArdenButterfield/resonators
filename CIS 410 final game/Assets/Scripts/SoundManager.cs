using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource musicKickSource;
    public AudioSource car1FXSource;
    public AudioSource car2FXSource;

    public AudioClip[] coinPickups;
    public AudioClip musicLoop;
    public AudioClip musicKick;
    public AudioClip[] boostSounds;
    public AudioClip[] driftSounds;
    public AudioClip[] crashSounds;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.loop = true;
        musicKickSource.loop = true;
        musicSource.clip = musicLoop;
        musicKickSource.clip = musicKick;
        musicSource.Play();
        musicKickSource.Play();
    }

    // Called from Nitro Manager
    public void playPickup(int CarNumber) {
        AudioSource playSource;
        if (CarNumber == 1) {
            playSource = car1FXSource;
        } else {
            playSource = car2FXSource;
        }
        // Unity Random range is inclusive on the lower and upper bounds, which
        // is kind of weird, and why we need the modulo.
        int index = Random.Range(0,coinPickups.Length) % coinPickups.Length;

        playSource.PlayOneShot(coinPickups[index], 0.5f);
    }

    // Called from Nitro Manager
    public void playBoost(int CarNumber) {
        AudioSource playSource;
        if (CarNumber == 1) {
            playSource = car1FXSource;
        } else {
            playSource = car2FXSource;
        }
        int index = Random.Range(0, boostSounds.Length) % boostSounds.Length;
        playSource.PlayOneShot(boostSounds[index]);
    }

    public void playCrash(int CarNumber, float volume) {
        AudioSource playSource;
        if (CarNumber == 1) {
            playSource = car1FXSource;
        } else {
            playSource = car2FXSource;
        }
        int index = Random.Range(0, crashSounds.Length) % crashSounds.Length;
        playSource.PlayOneShot(crashSounds[index], volume);
    }
}
