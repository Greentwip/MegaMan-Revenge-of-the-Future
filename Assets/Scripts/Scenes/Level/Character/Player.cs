using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int jumpForce = 16000;
    public int walkSpeed = 450;
    public int slideSpeed = 1100;

    public int topWalkSpeed = 128;
    public int topSlideSpeed = 144;
    public float longSlideDuration = 0.5f;

    public int busterShootSpeed = 300;

    public AudioClip busterLowSound = null;
    public Rigidbody2D busterLowWeapon = null;


    public string titleScene;

    [System.NonSerialized]
    public bool isInPassage = false;

    [System.NonSerialized]
    public bool climbing = false;

    [System.NonSerialized]
    bool onLadder = false;

    [System.NonSerialized]
    public bool frozen = false;

    [System.NonSerialized]
    public bool startup = false;

    [System.NonSerialized]
    public bool morphing = false;

    [System.NonSerialized]
    public bool reverseMorphing = false;

    [System.NonSerialized]
    public bool teleporting = false;

    [System.NonSerialized]
    bool sliding = false;

    [System.NonSerialized]
    bool jumping = false;

    [System.NonSerialized]
    bool attacking = false;


    // timers and durations
    [System.NonSerialized]
    float attackTimer = 0.0f;

    [System.NonSerialized]
    float timeToStopAttack = 0.0f;

    [System.NonSerialized]
    const float shortSlideDuration = 0.1f;

    [System.NonSerialized]
    private int topVerticalSpeed = 345;


    // collisions
    [System.NonSerialized]
    public CollisionDetector obstacleDetector;

    Vector3 moveVelocity;

    Animator animator;

    Controller controller;

    List<Collider2D> attackingWeapons = new List<Collider2D>();
    List<Weapon> bulletsShot = new List<Weapon>();

    void VerifyDamage()
    {
        if (!GetComponent<DamageTrigger>().stunned && !GetComponent<DamageTrigger>().invulnerable)
        {
            if (attackingWeapons.Count != 0)
            {

                var weapon = attackingWeapons[0];

                if (attackingWeapons[0] != null)
                {
                    if (weapon.GetComponent<Weapon>().active)
                    {

                        if (GetComponent<DamageTrigger>().SurvivesDamage(weapon.GetComponent<Weapon>().power))
                        {
                            GetComponent<DamageTrigger>().TakeDamage(weapon.GetComponent<Weapon>().power);
                        }
                        else
                        {
                            GetComponent<DamageTrigger>().KillCharacter();
                        }
                    }
                }
                
            }
        }

    }

    public void OnAfterDead()
    {
        StartCoroutine(WaitAndResetRoutine());
    }

    protected IEnumerator WaitAndResetRoutine()
    {
        // before the wait

        var previousVelocity = GetComponent<Rigidbody2D>().velocity;

        previousVelocity.x = 0;
        previousVelocity.y = 0;

        GetComponent<Rigidbody2D>().velocity = previousVelocity;

        GetComponent<Rigidbody2D>().simulated = false;


        SoundManager.Instance.StopBGM();

        // start the wait
        yield return new WaitForSeconds(3.6f);

        // after the wait

        if (GetComponent<SpriteRenderer>().flipX)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        CurrentGame.Instance.PlayerLives -= 1;

        if (CurrentGame.Instance.PlayerLives == 0)
        {
            // we will swap the scene to the title if we ran out of lives
            SceneSwapper.Instance.SwapScene(titleScene);
        }
        else
        {
            // FadeManager.Instance.Fade will execute OnDeathEndReset after fade
            FadeManager.Instance.Fade(true, 1.0f, OnDeathBeginReset);
        }

        
    }

    void OnDeathBeginReset()
    {

        // we reset the enemies
        var enemies = FindObjectsOfType<Enemy>();

        foreach (var enemy in enemies)
        {
            enemy.Restart();
        }

        // we open the boss door again
        var bossDoors = FindObjectsOfType<BossDoor>();


        foreach (var bossDoor in bossDoors)
        {
            bossDoor.Open();
        }
        


        // we get the last checkpoint position
        var checkpointPosition =
            CurrentGame.Instance.CurrentCheckpoint.transform.position;
        
        var cameraPosition = Camera.main.transform.position;

        var newPlayerPosition = checkpointPosition;

        newPlayerPosition.y += 144;

        this.transform.position = newPlayerPosition;

        cameraPosition.x = checkpointPosition.x;
        cameraPosition.y = checkpointPosition.y;

        Camera.main.transform.position = cameraPosition;

        FadeManager.Instance.Fade(false, 1.0f, OnDeathEndReset);
    }

    void OnDeathEndReset()
    {
        GetComponent<Rigidbody2D>().simulated = true;

        GetComponent<DamageTrigger>().SetVisible(true);

        GetComponent<Health>().Reset();

        FindObjectOfType<LevelManager>().StartLevel();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyWeapon")
        {
            attackingWeapons.Add(other);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "EnemyWeapon")
        {
            attackingWeapons.Remove(other);
        }

    }

    void Start()
    {
        obstacleDetector = GetComponent<CollisionDetector>();
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller>();

        timeToStopAttack = 45f / 60f;
    }

    void SetSlideCollisionBox()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(32, 16);
        collider.offset = new Vector2(0, 8);
    }

    void RestoreSlideCollisionBox()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(16, 32);
        collider.offset = new Vector2(0, 16);
    }

    IEnumerator Slide(float slideDuration)
    {
        SetSlideCollisionBox();
        obstacleDetector.Detect();
        this.sliding = true;
        yield return new WaitForSeconds(slideDuration);


        RestoreSlideCollisionBox();
        obstacleDetector.Detect();
        SetSlideCollisionBox();

        if (obstacleDetector.collisions.above)
        {
            StartCoroutine(Slide(shortSlideDuration));
        }
        else
        {
            if (this.sliding)
            {
                this.sliding = false;
                RestoreSlideCollisionBox();
            }
        }
    }

    void Update()
    {
        obstacleDetector.Detect();
       
        if (!this.frozen)
        {
            #region movement

            VerifyDamage();

            if (!GetComponent<DamageTrigger>().stunned)
            {

                int walk_direction = 0;

                if (controller.GetConditionButton("Right"))
                {
                    walk_direction = 1;
                }
                else if (controller.GetConditionButton("Left"))
                {
                    walk_direction = -1;
                }


                if (controller.GetConditionButton("Jump") &&
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

                if (controller.GetConditionButton("Down") &&
                    controller.GetConditionButtonDown("Jump") &&
                    GetComponent<Rigidbody2D>().velocity.y == 0.0f &&
                    !this.sliding)
                {
                    StartCoroutine(Slide(longSlideDuration));
                }

                if (this.sliding)
                {
                    if (GetComponent<SpriteRenderer>().flipX)
                    {
                        moveVelocity.x = -slideSpeed;

                        if (walk_direction == 1)
                        {
                            Vector3 velocity = GetComponent<Rigidbody2D>().velocity;

                            if (Mathf.Sign(velocity.x) == -1)
                            {
                                velocity.x *= -1;
                                moveVelocity.x = slideSpeed;
                            }

                            GetComponent<Rigidbody2D>().velocity = velocity;
                        }
                    }
                    else
                    {
                        moveVelocity.x = slideSpeed;

                        if (walk_direction == -1)
                        {
                            Vector3 velocity = GetComponent<Rigidbody2D>().velocity;

                            if (Mathf.Sign(velocity.x) == 1)
                            {
                                velocity.x *= -1;
                                moveVelocity.x = -slideSpeed;
                            }

                            GetComponent<Rigidbody2D>().velocity = velocity;
                        }
                    }

                }
                else
                {
                    moveVelocity.x = walk_direction * walkSpeed;
                } 
            } 
            else
            {
                if (!this.sliding)
                {
                    if (GetComponent<SpriteRenderer>().flipX)
                    {
                        Vector3 velocity = GetComponent<Rigidbody2D>().velocity;

                        velocity.x = 0;
                        moveVelocity.x = (walkSpeed);

                        GetComponent<Rigidbody2D>().velocity = velocity;
                    }
                    else
                    {
                        Vector3 velocity = GetComponent<Rigidbody2D>().velocity;

                        velocity.x = 0;
                        moveVelocity.x = -(walkSpeed);

                        GetComponent<Rigidbody2D>().velocity = velocity;
                    }

                }
            }

            if (this.onLadder)
            {
                if (controller.GetConditionButtonDown("Jump"))
                {

                }
            }

            #endregion

            #region attack
            this.Attack();
            #endregion
        }
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
            StopCoroutine(Slide(longSlideDuration));
            RestoreSlideCollisionBox();
            this.sliding = false;
        }

        if (obstacleDetector.collisions.left && moveVelocity.x <= 0)
        {
            moveVelocity.x = 0;
            transformVelocity.x = 0;
            StopCoroutine(Slide(longSlideDuration));
            RestoreSlideCollisionBox();
            this.sliding = false;
        }

        if(transformVelocity.y > 0)
        {
            StopCoroutine(Slide(longSlideDuration));
            RestoreSlideCollisionBox();
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

        if(moveVelocity.x == 0 && !this.sliding)
        {
            transformVelocity.x = 0;
        }

        GetComponent<Rigidbody2D>().velocity = transformVelocity;

        #endregion

        #region force
        GetComponent<Rigidbody2D>().AddForce(moveVelocity);
        #endregion


        #region animations

        if (!this.startup)
        {
            if (transformVelocity.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (transformVelocity.x > 0)
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

            if (GetComponent<DamageTrigger>().stunned)
            {
                animator.SetBool("Stunned", true);
            }
            else
            {
                animator.SetBool("Stunned", false);
            }
        }
        

        
        #endregion

    }
    private void Attack()
    {

        List<Weapon> bulletsToRemove = new List<Weapon>();

        foreach (var bulletWeapon in this.bulletsShot)
        {
            if (bulletWeapon == null)
            {
                bulletsToRemove.Add(bulletWeapon);
            }
        }

        foreach(var bulletToRemove in bulletsToRemove)
        {
            this.bulletsShot.Remove(bulletToRemove);
        }

        if (Input.GetButtonDown("Fire"))
        {
            if(bulletsShot.Count < 3 && !this.sliding)
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

                    Vector3 transformVelocity = GetComponent<Rigidbody2D>().velocity;

                    if (transformVelocity.y != 0.0f)
                    {
                        bulletPosition.x += 8;
                        bulletPosition.y += 12;
                    }
                }
                else
                {
                    bulletPosition.x += 24;
                    Vector3 transformVelocity = GetComponent<Rigidbody2D>().velocity;

                    if (transformVelocity.y != 0.0f)
                    {
                        bulletPosition.x -= 8;
                        bulletPosition.y += 12;
                    }
                }


                Rigidbody2D bullet = Instantiate(busterLowWeapon,
                                                 bulletPosition,
                                                 transform.rotation);

                var bulletComponent = bullet.GetComponent<Bullet>();

                this.bulletsShot.Add(bulletComponent);

                var velocity = bullet.velocity;

                if (this.GetComponent<SpriteRenderer>().flipX)
                {
                    velocity.x = -busterShootSpeed;
                }
                else
                {
                    velocity.x = busterShootSpeed;
                }




                bullet.velocity = velocity;

                SoundManager.Instance.PlaySingle(busterLowSound);

            }


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
}
