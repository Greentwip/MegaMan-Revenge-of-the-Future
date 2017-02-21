using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGame : MonoBehaviour
{

    public static CurrentGame Instance { set; get; }     //Allows other scripts to call functions from SoundManager.             


    public Checkpoint CurrentCheckpoint { set; get; }
    public uint PlayerLives { set; get; }
    public bool IsBossDefeated { set; get; }
    public float BGMVolume { set; get; }
    public float SFXVolume { set; get; }

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (Instance == null)
        {
            //if not, set it to this.
            Instance = this;

        }
        //If instance already exists:
        else if (Instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        this.CurrentCheckpoint = null;
        this.PlayerLives = 3;
        this.IsBossDefeated = false;
        this.BGMVolume = 100.0f;
        this.SFXVolume = 100.0f;
    }

}

