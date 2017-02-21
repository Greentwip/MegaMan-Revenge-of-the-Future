using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int jumpForce = 16000;
    public int walkSpeed = 900;
    public int slideSpeed = 2200;

    public int topWalkSpeed = 128;
    public int topSlideSpeed = 144;
    public float slideDuration = 0.5f;

    private int topVerticalSpeed = 345;


    [System.NonSerialized]
    public bool isInPassage = false;

    [System.NonSerialized]
    public bool climbing = false;

    [System.NonSerialized]
    public bool frozen = false;

    [System.NonSerialized]
    public bool morphing = false;

    [System.NonSerialized]
    public bool reverseMorphing = false;

    [System.NonSerialized]
    public bool teleporting = false;

    [System.NonSerialized]
    public CollisionDetector obstacleDetector;

    public AudioClip busterLowSound = null;
    public Rigidbody2D busterLowWeapon = null;


    Vector3 moveVelocity;

    Animator animator;

    Controller controller;


    bool sliding = false;
    bool jumping = false;
    bool attacking = false;

    float attackTimer;
    float timeToStopAttack;

    void Start()
    {
        obstacleDetector = GetComponent<CollisionDetector>();
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller>();

        timeToStopAttack = 0.16f + 0.1f;
    }

    IEnumerator Slide(float slideDuration)
    {
        this.sliding = true;
        yield return new WaitForSeconds(slideDuration);

        if (this.sliding)
        {
            this.sliding = false;
        }

    }

    void FixedUpdate()
    {
        obstacleDetector.Detect();
       
        if (!this.frozen)
        {
            #region movement

            int walk_direction = 0;

            if (controller.GetConditionButton("Right"))
            {
                walk_direction = 1;
            } else if (controller.GetConditionButton("Left"))
            {
                walk_direction = -1;
            }


            if (controller.GetConditionButtonDown("Jump") && 
                !controller.GetConditionButton("Down") &&
                GetComponent<Rigidbody2D>().velocity.y == 0.0f && 
                !this.jumping)
            {
                moveVelocity.y = jumpForce;
                this.jumping = true;
                this.sliding = false;
            }
            else
            {
                moveVelocity.y = 0;
                this.jumping = false;
            }

            if(controller.GetConditionButton("Down") &&
                controller.GetConditionButtonDown("Jump") &&
                GetComponent<Rigidbody2D>().velocity.y == 0.0f &&
                !this.sliding)
            {
                StartCoroutine(Slide(slideDuration));
            }

            if (this.sliding)
            {
                if (GetComponent<SpriteRenderer>().flipX)
                {
                    moveVelocity.x = -slideSpeed;
                }
                else
                {
                    moveVelocity.x = slideSpeed;
                }
            
            }
            else
            {
                moveVelocity.x = walk_direction * walkSpeed;
            }

            #endregion

        #region attack
            this.Attack();
        #endregion

        #region tweaks

            Vector3 transformVelocity = GetComponent<Rigidbody2D>().velocity;

            if (Mathf.Abs(transformVelocity.x) > topWalkSpeed && !this.sliding)
            {
                transformVelocity.x = topWalkSpeed * Mathf.Sign(transformVelocity.x);
            }

            if (Mathf.Abs(transformVelocity.x) > topSlideSpeed && this.sliding)
            {
                transformVelocity.x = topSlideSpeed * Mathf.Sign(transformVelocity.x);
            }

            if (Mathf.Abs(transformVelocity.y) > topVerticalSpeed)
            {
                transformVelocity.y = topVerticalSpeed * Mathf.Sign(transformVelocity.y);
            }

            if (obstacleDetector.collisions.right && moveVelocity.x >= 0)
            {
                moveVelocity.x = 0;
                transformVelocity.x = 0;
                this.sliding = false;
            }

            if (obstacleDetector.collisions.left && moveVelocity.x <= 0)
            {
                moveVelocity.x = 0;
                transformVelocity.x = 0;
                this.sliding = false;
            }

            if(transformVelocity.y != 0)
            {
                this.sliding = false;
            }
        
            if (controller.GetConditionButtonUp("Jump") && transformVelocity.y > 0.0f)
            {
                transformVelocity.y = -1;
            }

            if (obstacleDetector.collisions.above || obstacleDetector.collisions.below)
            {
                transformVelocity.y = 0;
            }

            GetComponent<Rigidbody2D>().velocity = transformVelocity;

        #endregion

        #region force
            GetComponent<Rigidbody2D>().AddForce(moveVelocity);
        #endregion


        #region animations
      
            if (transformVelocity.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(transformVelocity.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if (transformVelocity.x != 0 && transformVelocity.y == 0 && !sliding)
            {
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }

            if (transformVelocity.x != 0 && transformVelocity.y == 0 && sliding)
            {
                animator.SetBool("Sliding", true);
            }
            else
            {
                animator.SetBool("Sliding", false);
            }

            if (transformVelocity.y != 0)
            {
                animator.SetBool("Jumping", true);
            }
            else
            {
                animator.SetBool("Jumping", false);
            }

            if (this.attacking)
            {
                animator.SetBool("Attacking", true);
            }
            else
            {
                animator.SetBool("Attacking", false);
            }

        }
        #endregion

    }
    private void Attack()
    {
        if (Input.GetButtonDown("Fire"))
        {
            if (this.attacking)
            {
                attackTimer = 0;
            }
            else
            {
                this.attacking = true;
                attackTimer = 0;
            }


            var collider = GetComponent<Collider2D>();

            Vector3 bulletPosition = collider.bounds.center;

            bulletPosition.y += 2;

            if (this.GetComponent<SpriteRenderer>().flipX)
            {
                bulletPosition.x -= 24;
            }
            else
            {
                bulletPosition.x += 24;
            }


            Rigidbody2D bullet = (Rigidbody2D)Instantiate(busterLowWeapon,
                                                            bulletPosition,
                                                            transform.rotation);

            var velocity = bullet.velocity;

            if (this.GetComponent<SpriteRenderer>().flipX)
            {
                velocity.x = -200;
            }
            else
            {
                velocity.x = 200;
            }
            

            bullet.velocity = velocity;

            SoundManager.Instance.PlaySingle(busterLowSound);

        }

        if (this.attacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= timeToStopAttack)
            {
                this.attacking = false;
            }
        }
        
    }

    private void Charge()
    {

    }

    public void Kill()
    {
        TakeDamage(100);
    }

    void TakeDamage(float damage)
    {
        GetComponent<Health>().ChangeHealth(-damage);
    }
}
