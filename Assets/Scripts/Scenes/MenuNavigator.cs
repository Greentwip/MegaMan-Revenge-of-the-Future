using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigator : MonoBehaviour {

    public Object previousScene;

    private bool navigating;

	// Use this for initialization
	void Start () {
        this.navigating = false;	
	}


    void SwitchLevel()
    {
        SceneManager.LoadScene(previousScene.name);
        FadeManager.Instance.Fade(false, 1.5f, null);
    }

    void SwitchLevelFade()
    {
        FadeManager.Instance.Fade(true, 1.5f, SwitchLevel);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!this.navigating && !FadeManager.Instance.isInTransition)
            {
                this.navigating = true;
                this.SwitchLevelFade();
            }
            

        }
    }
}
