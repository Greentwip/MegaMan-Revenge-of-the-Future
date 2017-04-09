using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitarTank : Enemy {

    public LayerMask collisionMask;

    public Rigidbody2D weapon;

    private bool attacking = false;

    IEnumerator AttackRoutine()
    {

        this.attacking = true;

        yield return new WaitForSeconds(1.5f);

        var bulletPosition = this.transform.position;

        bulletPosition.x -= 16;
        bulletPosition.y += 16;

        Rigidbody2D bullet = Instantiate(weapon,
                                         bulletPosition,
                                         transform.rotation);


        Vector2 moveVelocity = new Vector2(-2500, 12000);

        bullet.AddForce(moveVelocity);

        this.attacking = false;
    }

    protected override void OnDefeated()
    {
        base.OnDefeated();

        this.attacking = false;
    }

    protected override void Logic()
    {
        Vector2 rayOrigin = GetComponent<Collider2D>().bounds.center;
        rayOrigin.x = GetComponent<Collider2D>().bounds.min.x;

        float rayLength = 24;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin,
                                     Vector2.left,
                                     rayLength,
                                     collisionMask);

        Debug.DrawRay(rayOrigin, 
                      Vector2.left * (rayLength), 
                      Color.red);

        if (hit)
        {
            var velocity = GetComponent<Rigidbody2D>().velocity;

            velocity.x = 0;

            GetComponent<Rigidbody2D>().velocity = velocity;

            if (!this.attacking)
            {
                StartCoroutine(AttackRoutine());
            }

        } else
        {
            var velocity = GetComponent<Rigidbody2D>().velocity;

            velocity.x = -50;

            GetComponent<Rigidbody2D>().velocity = velocity;
        }

        
    }
}
