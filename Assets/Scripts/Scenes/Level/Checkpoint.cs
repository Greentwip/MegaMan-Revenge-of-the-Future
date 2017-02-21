using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "MainCamera")
        {
            CurrentGame.Instance.CurrentCheckpoint = this;
        }
    }
    
}
