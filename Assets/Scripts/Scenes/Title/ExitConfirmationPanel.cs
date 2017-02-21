using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitConfirmationPanel : MonoBehaviour {

    public EventTrigger buttonYesTrigger;
    public EventTrigger buttonNoTrigger;

    public bool onScreen { get; set; }

    private void Start()
    {
        EventTrigger.Entry yes_entry = new EventTrigger.Entry();
        yes_entry.eventID = EventTriggerType.PointerClick;
        yes_entry.callback.AddListener((data) => { OnExitYesDown((PointerEventData)data); });
        buttonYesTrigger.triggers.Add(yes_entry);

        EventTrigger.Entry no_entry = new EventTrigger.Entry();
        no_entry.eventID = EventTriggerType.PointerClick;
        no_entry.callback.AddListener((data) => { OnExitNoDown((PointerEventData)data); });
        buttonNoTrigger.triggers.Add(no_entry);

        this.onScreen = false;
    }

    public void OnExitYesDown(PointerEventData data)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnExitNoDown(PointerEventData data)
    {
        MoveOut();
    }

    public void MoveIn()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path",
                                        iTweenPath.GetPath("ExitConfirmationIn"),
                                        "time",
                                        1));

        this.onScreen = true;
    }

    public void MoveOut()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path",
                                      iTweenPath.GetPath("ExitConfirmationOut"),
                                      "time",
                                      1));

        this.onScreen = false;
    }
}
