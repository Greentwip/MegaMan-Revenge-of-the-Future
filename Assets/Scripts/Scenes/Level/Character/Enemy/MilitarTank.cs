using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitarTank : Enemy {

    public LayerMask collisionMask;

    public LayerMask groundCollisionMask;

    public Rigidbody2D weapon;

    private bool attacking = false;

    private int moveVelocity = -50;

    IEnumerator AttackRoutine()
    {

        this.attacking = true;

        yield return new WaitForSeconds(1.5f);

        var bulletPosition = this.transform.position;

        bulletPosition.x -= 16;

        if (GetComponent<SpriteRenderer>().flipX)
        {
            bulletPosition.y -= 16;
        }
        else
        {
            bulletPosition.y += 16;
        }
        

        Rigidbody2D bullet = Instantiate(weapon,
                                         bulletPosition,
                                         transform.rotation);

        int bulletMoveVelocity = 0;

        if (GetComponent<SpriteRenderer>().flipX)
        {
            bulletMoveVelocity = 2500;
        }
        else
        {
            bulletMoveVelocity = -2500;
        }


        Vector2 moveVelocity = new Vector2(bulletMoveVelocity, 12000);

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

        if (GetComponent<SpriteRenderer>().flipX)
        {
            rayOrigin.x = GetComponent<Collider2D>().bounds.max.x;
        }
        else
        {
            rayOrigin.x = GetComponent<Collider2D>().bounds.min.x;
        }

        

        float rayLength = 24;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin,
                                     GetComponent<SpriteRenderer>().flipX ? Vector2.right : Vector2.left,
                                     rayLength,
                                     collisionMask);

        Debug.DrawRay(rayOrigin, 
                      Vector2.left * (rayLength), 
                      Color.red);

        if (hit.collider != null)
        {
            var velocity = GetComponent<Rigidbody2D>().velocity;

            velocity.x = 0;

            GetComponent<Rigidbody2D>().velocity = velocity;

            if (!this.attacking)
            {
                StartCoroutine(AttackRoutine());
            }

        } 
        else
        {

            rayOrigin = GetComponent<Collider2D>().bounds.min;

            rayLength = 24;

            hit = Physics2D.Raycast(rayOrigin,
                                    Vector2.down,
                                    rayLength,
                                    groundCollisionMask);

            if (hit.collider == null)
            {
                moveVelocity = -moveVelocity;
            }

            rayOrigin = GetComponent<Collider2D>().bounds.center;

            rayOrigin.x = GetComponent<Collider2D>().bounds.min.x;

            rayLength = 12;

            hit = Physics2D.Raycast(rayOrigin,
                                    Vector2.left,
                                    rayLength,
                                    groundCollisionMask);

            if (hit.collider != null)
            {
                moveVelocity = -moveVelocity;
            }

            rayOrigin = GetComponent<Collider2D>().bounds.center;

            rayOrigin.x = GetComponent<Collider2D>().bounds.max.x;

            rayLength = 12;

            hit = Physics2D.Raycast(rayOrigin,
                                    Vector2.right,
                                    rayLength,
                                    groundCollisionMask);

            if (hit.collider != null)
            {
                moveVelocity = -moveVelocity;
            }


            var velocity = GetComponent<Rigidbody2D>().velocity;

            velocity.x = moveVelocity;

            GetComponent<Rigidbody2D>().velocity = velocity;
        }

        GetComponent<SpriteRenderer>().flipX = moveVelocity > 0;

    }
}
