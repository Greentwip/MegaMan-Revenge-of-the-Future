using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapper : MonoBehaviour
{

    public static SceneSwapper Instance { set; get; }

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

    void SwitchLevel(Object scene)
    {
        SceneManager.LoadScene(scene.name);
        FadeManager.Instance.Fade(false, 1.5f, null);
    }

    void SwitchLevelFade(Object scene)
    {
        FadeManager.Instance.Fade(true, 1.5f, SwitchLevel, scene);
    }

    public void SwapScene(Object scene)
    {
        SwitchLevelFade(scene);
    }

}
