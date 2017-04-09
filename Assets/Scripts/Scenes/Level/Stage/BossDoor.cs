using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour {

    public AudioClip closeSound = null;

    Animator animator = null;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    public void Open()
    {
        animator.SetBool("Closed", false);
        this.GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var playerTransform = other.transform;

            if(playerTransform.position.x > this.transform.position.x)
            {
                this.GetComponent<Collider2D>().isTrigger = false;
                animator.SetBool("Closed", true);

                SoundManager.Instance.PlaySingle(closeSound);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
