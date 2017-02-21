using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {

    public static FadeManager Instance{ set; get;}

    public Image fadeImage;

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;
    private Color fadeColor;

    public delegate void onFadeEndDelegate();
    public delegate void onFadeEndWithParametersDelegate(Object parameters);

    onFadeEndDelegate onFadeEnd;
    onFadeEndWithParametersDelegate onFadeEndWithParameters;

    Object parameters;

    bool isFadeWithParameters;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (Instance == null)
            //if not, set it to this.
            Instance = this;
        //If instance already exists:
        else if (Instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void Fade(bool showing, float duration, onFadeEndDelegate onFadeEnd)
    {
        this.isShowing = showing;
        this.isInTransition = true;
        this.duration = duration;
        this.transition = (isShowing) ? 0 : 1;
        this.onFadeEnd = onFadeEnd;
        this.fadeColor = this.fadeImage.color;

        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 1024;

        isFadeWithParameters = false;
    }

    public void Fade(bool showing, 
                     float duration, 
                     onFadeEndWithParametersDelegate onFadeEnd, 
                     Object parameters)
    {
        this.isShowing = showing;
        this.isInTransition = true;
        this.duration = duration;
        this.transition = (isShowing) ? 0 : 1;
        this.fadeColor = this.fadeImage.color;

        this.onFadeEndWithParameters = onFadeEnd;
        this.parameters = parameters;

        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 1024;

        isFadeWithParameters = true;
    }

    // Update is called once per frame
    void Update () {

        if (this.isInTransition)
        {
            this.transition += (isShowing) ? Time.deltaTime * (1 / this.duration) :
                                            -Time.deltaTime * (1 / this.duration);

            this.fadeImage.color = 
                Color.Lerp(new Color(this.fadeColor.r,
                                     this.fadeColor.g,
                                     this.fadeColor.b, 
                                     0),
                           new Color(this.fadeColor.r,
                                     this.fadeColor.g,
                                     this.fadeColor.b,
                                     1), 
                           this.transition);

            if(this.transition > 1 || this.transition < 0)
            {
                this.isInTransition = false;
                Canvas canvas = GetComponent<Canvas>();
                canvas.sortingOrder = -1024;

                if (isFadeWithParameters)
                {
                    if (this.onFadeEndWithParameters != null)
                    {
                        this.onFadeEndWithParameters(this.parameters);
                    }
                }
                else
                {
                    if (this.onFadeEnd != null)
                    {
                        this.onFadeEnd();
                    }
                }
                
            }
        }
    }
}
