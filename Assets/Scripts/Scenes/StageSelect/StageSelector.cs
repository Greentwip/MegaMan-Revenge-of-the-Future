using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour {


    public Transform iceman;
    public Transform sheriffman;
    public Transform boomerman;
    public Transform militaryman;
    public Transform vineman;
    public Transform windman;
    public Transform nightman;
    public Transform fastman;
    public Transform drwilly;

    public AudioClip selectSound;
    public AudioClip selectedSound;

    private Transform currentTransform;

    private SpriteRenderer sr;

    float counter; 

	// Use this for initialization
	void Start () {
        sr = this.GetComponent<SpriteRenderer>();
        counter = 0.0f;

        currentTransform = drwilly;
	}
	
	// Update is called once per frame
	void Update () {

        counter += Time.deltaTime;

        if(counter >= 0.2f && counter <= 0.4f)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        }
        else if(counter <= 0.2f)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        }

        if(counter > 0.4f)
        {
            counter = 0.0f;
        }

        if(Application.platform == RuntimePlatform.WindowsEditor
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.WSAPlayerX86
            || Application.platform == RuntimePlatform.LinuxEditor
            || Application.platform == RuntimePlatform.LinuxPlayer
            || Application.platform == RuntimePlatform.OSXEditor
            || Application.platform == RuntimePlatform.OSXPlayer)
        {
            if(Input.GetButtonDown("Right"))
            {
                if(currentTransform == drwilly
                    || currentTransform == boomerman
                    || currentTransform == fastman)
                {
                    MoveToTransform(vineman);
                }

                if(currentTransform == sheriffman)
                {
                    MoveToTransform(boomerman);
                }

                if(currentTransform == nightman)
                {
                    MoveToTransform(fastman);
                }

                if(currentTransform == iceman)
                {
                    MoveToTransform(sheriffman);
                }

                if(currentTransform == windman)
                {
                    MoveToTransform(nightman);
                }

                if(currentTransform == militaryman)
                {
                    MoveToTransform(drwilly);
                }
            }

            if (Input.GetButtonDown("Left"))
            {
                if (currentTransform == drwilly
                    || currentTransform == iceman
                    || currentTransform == windman)
                {
                    MoveToTransform(militaryman);
                }

                if (currentTransform == sheriffman)
                {
                    MoveToTransform(iceman);
                }

                if (currentTransform == windman)
                {
                    MoveToTransform(militaryman);
                }

                if (currentTransform == nightman)
                {
                    MoveToTransform(windman);
                }

                if (currentTransform == boomerman)
                {
                    MoveToTransform(sheriffman);
                }

                if(currentTransform == fastman)
                {
                    MoveToTransform(nightman);
                }

                if(currentTransform == vineman)
                {
                    MoveToTransform(drwilly);
                }

            }

            if(Input.GetButtonDown("Up"))
            {
                if(currentTransform == drwilly)
                {
                    MoveToTransform(sheriffman);
                }

                if(currentTransform == windman)
                {
                    MoveToTransform(iceman);
                }

                if(currentTransform == nightman)
                {
                    MoveToTransform(drwilly);
                }

                if(currentTransform == fastman)
                {
                    MoveToTransform(boomerman);
                }
            }

            if (Input.GetButtonDown("Down"))
            {
                if(currentTransform == drwilly)
                {
                    MoveToTransform(nightman);
                }

                if(currentTransform == iceman)
                {
                    MoveToTransform(windman);
                }

                if(currentTransform == boomerman)
                {
                    MoveToTransform(fastman);
                }

                if(currentTransform == sheriffman)
                {
                    MoveToTransform(drwilly);
                }
            }
        }

	}

    void MoveToTransform(Transform new_transform)
    {
        this.currentTransform = new_transform;
        this.transform.position = this.currentTransform.position;
        SoundManager.Instance.PlaySingle(selectSound);
    }
}
