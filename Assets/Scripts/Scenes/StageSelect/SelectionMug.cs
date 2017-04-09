using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionMug : MonoBehaviour, IPointerDownHandler {

    public string level = "";

    public Transform selector = null;

    public string introScene;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            if (CurrentGame.Instance.CurrentLevel != this.level)
            {
                CurrentGame.Instance.CurrentLevel = this.level;
                selector.GetComponent<StageSelector>().MoveToTransform(this.transform);
            }
            else
            {
                selector.GetComponent<StageSelector>().SwapScene(this.introScene);
            }

        }
    }
}
