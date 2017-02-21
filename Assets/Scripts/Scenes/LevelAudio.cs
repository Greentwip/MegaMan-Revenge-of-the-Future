using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudio : MonoBehaviour {

    public AudioClip music;
    public bool isLoop;

	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlayBGM(music, isLoop);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
