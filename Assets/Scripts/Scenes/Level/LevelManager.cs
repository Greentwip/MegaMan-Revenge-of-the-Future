using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    private Player player;

    [SerializeField]
    private Text readyText;

    [SerializeField]
    private AudioClip victoryMusic;

    bool playerMorphBegin = false;
    bool playerMophing = false;

    enum SequenceState
    {
        TeleportIn,
        MorphIn,
        VictoryMusic,
        MorphOut,
        PostMorphOut,
        TeleportOut,
        None
    }

    SequenceState sequenceState;

    static LevelManager Instance { get; set; }

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

        //Set to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start () {
        sequenceState = SequenceState.None;

        BeginTeleportIn();
	}

    void BeginTeleportIn()
    {
        sequenceState = SequenceState.TeleportIn;
        StartCoroutine(TeleportRoutineIn());
    }

    void BeginTeleportOut()
    {
        sequenceState = SequenceState.VictoryMusic;
    }

    IEnumerator TeleportRoutineIn()
    {
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        var initialColor = readyText.color;

        initialColor.a = 1.0f;

        readyText.color = initialColor;

        for (uint i = 0; i<9; ++i)
        {
            var color = readyText.color;

            yield return new WaitForSeconds(0.3f);

            if(color.a == 0.0f)
            {
                color.a = 1.0f;
            }
            else if (color.a == 1.0f)
            {
                color.a = 0.0f;
            }

            readyText.color = color;

        }

        player.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    IEnumerator MorphEndRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        player.teleporting = false;
        player.morphing = false;
        player.frozen = false;
    }

    void OnMusicEnd()
    {
        player.frozen = true;
        player.morphing = false;
        player.teleporting = false;
        player.reverseMorphing = true;

        sequenceState = SequenceState.MorphOut;

    }

    IEnumerator MorphBeginRoutine(float time)
    {


        yield return new WaitForSeconds(time);

        player.frozen = true;
        player.morphing = false;
        player.teleporting = true;
        player.reverseMorphing = false;

        var velocity = player.GetComponent<Rigidbody2D>().velocity;

        velocity.y = 256;

        player.GetComponent<Rigidbody2D>().velocity = velocity;

    }

    private void Update()
    {

        var playerAnimator = player.GetComponent<Animator>();

        if (player.teleporting)
        {
            playerAnimator.SetBool("Teleporting", true);
        }
        else
        {
            playerAnimator.SetBool("Teleporting", false);
        }

        if (player.morphing)
        {
            playerAnimator.SetBool("Morphing", true);
        }
        else
        {
            playerAnimator.SetBool("Morphing", false);
        }

        if (player.reverseMorphing)
        {
            playerAnimator.SetBool("Reverse Morphing", true);
        }
        else
        {
            playerAnimator.SetBool("Reverse Morphing", false);
        }


    }

    private void LateUpdate()
    {
        var playerAnimator = player.GetComponent<Animator>();


        switch (sequenceState)
        {
            case SequenceState.TeleportIn:
                if (player.obstacleDetector.collisions.below)
                {
                    player.morphing = true;
                    sequenceState = SequenceState.MorphIn;
                }
                else
                {
                    player.frozen = true;
                    player.teleporting = true;
                    player.morphing = false;
                }
                break;

            case SequenceState.MorphIn:
                {
                    var info =
                    playerAnimator.GetCurrentAnimatorClipInfo(0);

                    var clipLength = info[0].clip.length;

                    var coroutine = MorphEndRoutine(clipLength);

                    StartCoroutine(coroutine);

                    sequenceState = SequenceState.None;
                }

                break;

            case SequenceState.VictoryMusic:
                player.frozen = true;
                player.GetComponent<Rigidbody2D>().isKinematic = true;

                var velocity = player.GetComponent<Rigidbody2D>().velocity;
                velocity.x = 0;
                velocity.y = 0;
                player.GetComponent<Rigidbody2D>().velocity = velocity;

                SoundManager.Instance.PlayBGM(victoryMusic, false, OnMusicEnd);
                sequenceState = SequenceState.None;
                break;

            case SequenceState.MorphOut:
                {
                    // give one extra cycle for animations to set
                    sequenceState = SequenceState.PostMorphOut;
                }
                break;
            case SequenceState.PostMorphOut:
                {
                    var info =
                    playerAnimator.GetCurrentAnimatorClipInfo(0);

                    var clipLength = info[0].clip.length;

                    var coroutine = MorphBeginRoutine(clipLength);

                    StartCoroutine(coroutine);
                }
                break;
            case SequenceState.TeleportOut:
                if (player.obstacleDetector.collisions.below)
                {
                    player.reverseMorphing = true;
                }
                else
                {
                    player.frozen = true;
                    player.teleporting = true;
                    player.reverseMorphing = false;
                }
                break;

        }
    }
}
