using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float completionTime = 4.0f;


    //Hashtable onPlatform = new Hashtable();

    Player player;


    bool triggered = false;

    // Use this for initialization
    void Start () {
        Animator controller = GetComponent<Animator>();
        controller.SetBool("Playing", true);

	}

    void OnMoveBegin()
    {
        
        iTween.MoveTo(this.gameObject, iTween.Hash("path",
                        iTweenPath.GetPath(GetComponent<iTweenPath>().pathName),
                        "time",
                        completionTime,
                        "easeType",
                        iTween.EaseType.linear,
                        "onComplete",
                        "OnMoveEnd",
                        "oncompletetarget",
                        this.gameObject));

    }
    void OnMoveEnd()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("path",
                        iTweenPath.GetPathReversed(GetComponent<iTweenPath>().pathName),
                        "time",
                        completionTime,
                        "easeType",
                        iTween.EaseType.linear,
                        "onComplete",
                        "OnMoveBegin",
                        "oncompletetarget",
                        this.gameObject));

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            player = other.collider.GetComponent<Player>();
            player.transform.parent = this.transform;
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            player.transform.parent = null;
            player = null;
        }

    }

    void FixedUpdate()
    {

        if (player != null)
        {

            var rigidBody = player.GetComponent<Rigidbody2D>();

            //var yVelocity = this.GetComponent<Rigidbody2D>().velocity.y;

            // check if char seems to be jumping
            if (rigidBody.velocity.y == 0.0f && player.transform.position.y >=
                this.transform.position.y +
                this.GetComponent<BoxCollider2D>().bounds.extents.y &&
                player.transform.position.x >= this.transform.position.x - 
                this.GetComponent<BoxCollider2D>().bounds.extents.x &&
                player.transform.position.x <= this.transform.position.x + 
                this.GetComponent<BoxCollider2D>().bounds.extents.x)
            {

                var position = player.transform.position;

                position.y = this.transform.position.y +
                             this.GetComponent<BoxCollider2D>().bounds.extents.y;

                var velocity = rigidBody.velocity;

                velocity.y = 0;

                player.transform.position = position;
                rigidBody.velocity = velocity;

                if (!this.triggered)
                {
                    this.triggered = true;

                    OnMoveBegin();

                }
            }
        }

    }

}
