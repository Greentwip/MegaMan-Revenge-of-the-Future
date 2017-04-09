using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public static SoundManager Instance { set; get; }     //Allows other scripts to call functions from SoundManager.             

    public delegate void OnSoundEndDelegate();

    private GameObject BGM;

	void Awake()
	{
		//Check if there is already an instance of SoundManager
		if (Instance == null)
        {
            //if not, set it to this.
            Instance = this;
            BGM = new GameObject("BGM");
            BGM.AddComponent<AudioSource>();
            BGM.transform.parent = this.transform;

        }
        //If instance already exists:
        else if (Instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy(gameObject);

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);

        
    }

    public void PlayBGM(AudioClip clip, bool is_loop)
    {
        AudioSource source = BGM.GetComponent<AudioSource>();
        source.clip = clip;
        source.loop = is_loop;
        source.Play();
    }

    public void StopBGM()
    {
        AudioSource source = BGM.GetComponent<AudioSource>();
        source.Stop();
    }

    public void PlayBGM(AudioClip clip, bool is_loop, OnSoundEndDelegate onSoundEnd)
    {
        PlayBGM(clip, is_loop);
        StartCoroutine(OnSoundEndCoroutine(clip.length, onSoundEnd));
    }

    IEnumerator OnSoundEndCoroutine(float waitTime, OnSoundEndDelegate onSoundEnd)
    {
        yield return new WaitForSeconds(waitTime);
        
        if(onSoundEnd != null) // null propagating operator not available in .NET 4
        {
            onSoundEnd();
        }
    }

    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip)
	{
		GameObject go = new GameObject("Audio: " + clip.name);

        go.transform.parent = this.transform.parent;

		//Create the source
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
        source.volume = volumeSfx;
        source.Play();
		Destroy(go, clip.length);        

        //@TODO
        //set the volume
	}

    public void SetVolumeSfx(float volume)
    {
        volumeSfx = volume;
    }

    public void SetVolumeBgm(float volume)
    {
        AudioSource source = BGM.GetComponent<AudioSource>();
        source.volume = volume;
    }

    float volumeSfx = 1.0f;
}

