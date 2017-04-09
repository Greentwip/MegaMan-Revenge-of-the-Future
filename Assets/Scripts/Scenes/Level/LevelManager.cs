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

    // Use this for initialization
    void Start () {
        StartLevel();
	}

    // start level
    public void StartLevel()
    {
        player.startup = true;

        GetComponent<LevelAudio>().PlayMusicBgm();
        sequenceState = SequenceState.None;
        BeginTeleportIn();
    }

    void BeginTeleportIn()
    {
        sequenceState = SequenceState.TeleportIn;
        StartCoroutine(TeleportInRoutine());
    }
        
    IEnumerator TeleportInRoutine()
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

    IEnumerator MorphInEndRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        player.teleporting = false;
        player.morphing = false;
        player.frozen = false;
        player.startup = false;
    }

    // finish level

    public void FinishLevel()
    {
        player.frozen = true;

        /*if (CurrentGame.Instance.CurrentLevel.IsBossBeaten)
        {
            
        }
        else
        {
            
        }*/
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

                var coroutine = MorphInEndRoutine(clipLength);

                StartCoroutine(coroutine);

                sequenceState = SequenceState.None;
            }
            break;


        }
    }
}
