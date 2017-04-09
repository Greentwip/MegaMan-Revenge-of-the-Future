using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenGrenader : Enemy {

    public Rigidbody2D weapon;

    public AnimationClip attackAnimation;

    Animator animator;

    private bool attacking = false;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
    }

    IEnumerator AttackRoutine()
    {
        this.attacking = true;

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Attacking", true);

        yield return new WaitForSeconds(attackAnimation.length * 0.75f);

        var bulletPosition = this.transform.position;

        bulletPosition.x -= 16;
        bulletPosition.y += 16;

        Rigidbody2D bullet = Instantiate(weapon,
                                         bulletPosition,
                                         transform.rotation);


        Vector2 moveVelocity = new Vector2(-2500, 12000);

        bullet.AddForce(moveVelocity);

        yield return new WaitForSeconds(attackAnimation.length * 0.25f);


        this.attacking = false;

        animator.SetBool("Attacking", false);

    }

    protected override void OnDefeated()
    {
        base.OnDefeated();

        this.attacking = false;
    }

    protected override void Logic()
    {

        if (!this.attacking)
        {
            StartCoroutine(AttackRoutine());
        }

    }
}
