using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleCanvas : MonoBehaviour {

    public AudioClip selectedSound;

    public ExitConfirmationPanel exitConfirmationPanel;

    public Object nextLevel;

    private bool transitioning;

    // Use this for initialization
    void Start () {

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);

        this.transitioning = false;
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        if (!this.transitioning)
        {
            this.transitioning = true;
            SoundManager.Instance.PlaySingle(selectedSound);

            SwapScene();
        }

    }
    
    void SwapScene()
    {
        SceneSwapper.Instance.SwapScene(nextLevel);
    }


    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (exitConfirmationPanel.onScreen)
            {
                exitConfirmationPanel.MoveOut();
            }
            else
            {
                exitConfirmationPanel.MoveIn();
            }

        }
    }
}   
