using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OpeningTransition : MonoBehaviour {

    public string nextLevel;

	// Use this for initialization
	void Start () {

        Color fade_color = FadeManager.Instance.fadeImage.color;
        FadeManager.Instance.fadeImage.color = new Color(fade_color.r, 
                                                         fade_color.g, 
                                                         fade_color.b, 
                                                         1.0f);


        Transition();


    }

    void SwapScene()
    {
        SceneSwapper.Instance.SwapScene(nextLevel);
    }

    void FadeEnd()
    {
        Animation openingAnimation = GetComponent<Animation>();
        Animator openingController = GetComponent<Animator>();

        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = "SwapScene";
        animationEvent.time = openingAnimation.clip.length;     
        openingAnimation.clip.AddEvent(animationEvent);

        openingController.SetBool("playing", true);

    }

    void Transition()
    {
        FadeManager.Instance.Fade(false, 1.5f, FadeEnd);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
