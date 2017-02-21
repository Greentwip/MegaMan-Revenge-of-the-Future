using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitarTank : Enemy {
    protected override void Logic()
    {
        var velocity = GetComponent<Rigidbody2D>().velocity;

        velocity.x = -25;

        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
