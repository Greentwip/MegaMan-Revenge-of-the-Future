using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public enum Status
    {
        Active,
        Inactive,
        Fighting,
        Defeated
    }

    public uint health              = 10;
    public AudioClip damageSound    = null;
    public AudioClip defeatedSound  = null;
    public bool dropsItem           = true;

    uint currentHealth              = 0;
    bool idleReappear               = false;

    Vector3 initialPosition         = Vector3.zero;

    public Status status;

    // Use this for initialization
    void Start () {
        this.currentHealth = health;
        //this.status = Status.Inactive;
        this.initialPosition = this.transform.position;
    }

    void UpdateStatus()
    {
        var mainCamera = Camera.main;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var testBounds = spriteRenderer.bounds;

        if(GeometryHelper.Contains(mainCamera, spriteRenderer.bounds))
        {
            if (this.status == Status.Active)
            {
                this.status = Status.Fighting;
                this.currentHealth = this.health;
            }
            else if (this.status == Status.Defeated)
            {
                this.OnDefeated();
                //this.SetVisible(false);
                //this.status = Status.Inactive;
                //this.transform.position = initialPosition;
            }
        }
        else
        {
            if(this.status == Status.Fighting || this.status == Status.Inactive)
            {

                testBounds.center = initialPosition;

                if (!GeometryHelper.Contains(mainCamera, testBounds))
                {
                    SetVisible(true);
                    this.status = Status.Active;
                    this.transform.position = initialPosition;
                }
            }
        }
        
    }

    // Update is called once per frame
    void FixedUpdate () {

        this.UpdateStatus();

        if (this.status == Status.Fighting)
        {
            Logic();
        }
	}

    private void SetVisible(bool visible)
    {
        var color = GetComponent<SpriteRenderer>().color;

        if (visible)
        {
            color.a = 1.0f;
        }
        else
        {
            color.a = 0.0f;
        }

        GetComponent<SpriteRenderer>().color = color;
    }

    private void OnDefeated()
    {
        Destroy(this.gameObject);
        //@TODO item spawn
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "PlayerWeapon")
        {
            if (this.status == Status.Fighting)
            {
                var bullet = other.GetComponent<Bullet>();

                TakeDamage(bullet.power);

                if (this.currentHealth > 0)
                {
                    SoundManager.Instance.PlaySingle(damageSound);
                    StartCoroutine(BlinkRoutine());
                }
                else
                {
                    SoundManager.Instance.PlaySingle(defeatedSound);
                    this.status = Status.Defeated;
                }
            }            
        }

    }

    private void TakeDamage(uint damage)
    {
        if(damage > this.currentHealth)
        {
            this.currentHealth = 0;
        }
        else
        {
            this.currentHealth -= damage;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        SetVisible(true);

        yield return new WaitForSeconds(0.1f);

        SetVisible(false);

        yield return new WaitForSeconds(0.1f);

        SetVisible(true);
    }

    protected abstract void Logic();
}
