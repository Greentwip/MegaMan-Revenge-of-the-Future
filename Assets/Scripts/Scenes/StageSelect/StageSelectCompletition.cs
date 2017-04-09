using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCompletition : MonoBehaviour {

    public Transform militaryman;
    public Transform sheriffman;
    public Transform nightman;
    public Transform vineman;

	// Use this for initialization
	void Start () {
        if (!CurrentGame.Instance.IsBossDefeated("militaryman"))
        {
            var color = militaryman.GetComponent<SpriteRenderer>().color;

            color.a = 0.0f;

            militaryman.GetComponent<SpriteRenderer>().color = color;
        }

        if (!CurrentGame.Instance.IsBossDefeated("sheriffman"))
        {
            var color = sheriffman.GetComponent<SpriteRenderer>().color;

            color.a = 0.0f;

            sheriffman.GetComponent<SpriteRenderer>().color = color;
        }

        if (!CurrentGame.Instance.IsBossDefeated("nightman"))
        {
            var color = nightman.GetComponent<SpriteRenderer>().color;

            color.a = 0.0f;

            nightman.GetComponent<SpriteRenderer>().color = color;
        }

        if (!CurrentGame.Instance.IsBossDefeated("vineman"))
        {
            var color = vineman.GetComponent<SpriteRenderer>().color;

            color.a = 0.0f;

            vineman.GetComponent<SpriteRenderer>().color = color;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
