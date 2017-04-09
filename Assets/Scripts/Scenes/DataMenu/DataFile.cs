using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DataFile : MonoBehaviour {

    public AudioClip selectionSound;
    public AudioClip selectedSound;

    public string file;

    public DataSelector dataSelector;

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


    void SwitchLevel()
    {
        SceneManager.LoadScene("Scenes/StageSelect");
        FadeManager.Instance.Fade(false, 1.5f, null);
    }

    void SwitchLevelFade()
    {
        FadeManager.Instance.Fade(true, 1.5f, SwitchLevel);
    }


    public void OnPointerDownDelegate(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            if (dataSelector.dataFile != this.file)
            {
                dataSelector.dataFile = this.file;
                CurrentGame.Instance.selectedDatafile = this.file;
                SoundManager.Instance.PlaySingle(selectionSound);
                dataSelector.transform.position = this.transform.position;
            }
            else
            {
                if (!this.transitioning)
                {
                    dataSelector.dataFile = this.file;
                    CurrentGame.Instance.selectedDatafile = this.file;

                    this.transitioning = true;
                    SoundManager.Instance.PlaySingle(selectedSound);
                    SwitchLevelFade();
                }
                
            }

        }
    }

}
