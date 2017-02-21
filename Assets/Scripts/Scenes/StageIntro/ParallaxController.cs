using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParallaxController : MonoBehaviour {

    public AudioClip bgmMusic;

    public Transform aParallax;
    public Transform bParallax;
    public Transform shadow;
    public Transform bossAnimation;

    public Text bossName;

    public Object levelScene;

    private string bossNameString;

    // Use this for initialization
    void Start () {
        this.bossNameString = bossName.text;

        bossName.text = "";

        OnMusicBegin();
        OnParallaxABegin();
        OnParallaxBBegin();
        OnShadowBegin();
    }

    void OnMusicBegin()
    {
        SoundManager.Instance.PlayBGM(bgmMusic, false, this.OnMusicEnd);
    }

    void OnMusicEnd()
    {
        SceneSwapper.Instance.SwapScene(levelScene);
    }
	
    void OnParallaxABegin()
    {
        iTween.MoveTo(aParallax.gameObject, iTween.Hash("path",
                                iTweenPath.GetPath("ParallaxA"),
                                "time",
                                1,
                                "easeType",
                                iTween.EaseType.linear,
                                "onComplete",
                                "OnParallaxAEnd",
                                "oncompletetarget",
                                this.gameObject));

    }
    void OnParallaxAEnd()
    {
        aParallax.position = new Vector3(aParallax.position.x,
                                         -224);
        iTween.MoveTo(aParallax.gameObject, iTween.Hash("path",
                                iTweenPath.GetPath("ParallaxB"),
                                "time",
                                1,
                                "easeType",
                                iTween.EaseType.linear,
                                "onComplete",
                                "OnParallaxABegin",
                                "oncompletetarget",
                                this.gameObject));

    }

    void OnParallaxBBegin()
    {
        bParallax.position = new Vector3(bParallax.position.x,
                                 -224);

        iTween.MoveTo(bParallax.gameObject, iTween.Hash("path",
                                iTweenPath.GetPath("ParallaxB"),
                                "time",
                                1,
                                "easeType",
                                iTween.EaseType.linear,
                                "onComplete",
                                "OnParallaxBEnd",
                                "oncompletetarget",
                                this.gameObject));

    }
    void OnParallaxBEnd()
    {
        iTween.MoveTo(bParallax.gameObject, iTween.Hash("path",
                                iTweenPath.GetPath("ParallaxA"),
                                "time",
                                1,
                                "easeType",
                                iTween.EaseType.linear,
                                "onComplete",
                                "OnParallaxBBegin",
                                "oncompletetarget",
                                this.gameObject));

    }

    void OnShadowBegin()
    {
        iTween.MoveTo(shadow.gameObject, iTween.Hash("path",
                        iTweenPath.GetPath("Shadow"),
                        "time",
                        1.5,
                        "easeType",
                        iTween.EaseType.linear,
                        "onComplete",
                        "OnShadowEnd",
                        "oncompletetarget",
                        this.gameObject));
    }

    void OnShadowEnd()
    {
        var animation_clip = bossAnimation.GetComponent<Animation>();
        Animator animation_controller = bossAnimation.GetComponent<Animator>();

        animation_controller.SetBool("playing", true);
        StartCoroutine(OnAnimationEnd(animation_clip.clip.length));

    }

    IEnumerator OnAnimationEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {        
        for (int i = 0; i < bossNameString.Length + 1; ++i)
        {
            bossName.text = bossNameString.Substring(0, i);
            yield return new WaitForSeconds(.2f);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
