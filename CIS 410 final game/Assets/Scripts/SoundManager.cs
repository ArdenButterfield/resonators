using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource musicKickSource;
    public AudioSource car1FXSource;
    public AudioSource car2FXSource;
    public AudioSource countdownSource;
    public AudioSource car1DriftSource;
    public AudioSource car2DriftSource;

    public AudioClip[] coinPickups;
    public AudioClip musicLoop;
    public AudioClip musicKick;
    public AudioClip[] boostSounds;
    public AudioClip[] driftSounds;
    public AudioClip[] crashSounds;
    public AudioClip[] respawnSounds;
    public AudioClip tick;
    public AudioClip drift;
    public float driftVolume;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.loop = true;
        musicKickSource.loop = true;
        car1DriftSource.loop = true;
        car2DriftSource.loop = true;

        car1DriftSource.clip = drift;
        car2DriftSource.clip = drift;

        musicSource.clip = musicLoop;
        musicKickSource.clip = musicKick;
        countdownSource.clip = tick;

        car1DriftSource.volume = 0f;
        car2DriftSource.volume = 0f;
        car1DriftSource.Play();
        car2DriftSource.Play();
    }


    public void playTick()
    {
        countdownSource.Play();
    }


    public void startTheMusic()
    {
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

        playSource.PlayOneShot(coinPickups[index], 0.3f);
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
        playSource.PlayOneShot(boostSounds[index],0.3f);
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

    public void playRespawn(int CarNumber) {
        AudioSource playSource;
        if (CarNumber == 1) {
            playSource = car1FXSource;
        } else {
            playSource = car2FXSource;
        }
        int index = Random.Range(0, respawnSounds.Length) % respawnSounds.Length;
        playSource.PlayOneShot(respawnSounds[index], 0.5f);
    }

    public void startDrift(int carNumber) 
    {
        //print("start drift");
        AudioSource playSource;
        if (carNumber == 1) {
            playSource = car1DriftSource;
        } else {
            playSource = car2DriftSource;
        }
        playSource.volume = driftVolume;
    }

    public void stopDrift(int carNumber) 
    {
        //print("stop drift");
        AudioSource playSource;
        if (carNumber == 1) {
            playSource = car1DriftSource;
        } else {
            playSource = car2DriftSource;
        }
        playSource.volume = 0f;
    }
}
