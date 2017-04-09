using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon {

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
