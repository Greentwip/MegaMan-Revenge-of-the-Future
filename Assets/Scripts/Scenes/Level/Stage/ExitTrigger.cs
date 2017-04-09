using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour {

    public AudioClip fanfareAudioClip = null;
    public string titleScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            var player = other.GetComponent<Player>();
            player.frozen = true;

            SoundManager.Instance.PlayBGM(fanfareAudioClip, false, OnFanfareEnd);
        }
    }

    void OnFanfareEnd()
    {
        SceneSwapper.Instance.SwapScene(titleScene);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
