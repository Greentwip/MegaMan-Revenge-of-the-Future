using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour {

    [SerializeField]
    protected AudioClip damageSoundEffect;

    [SerializeField]
    protected AudioClip deathSoundEffect;

    [SerializeField]
    protected Rigidbody2D deathParticlePrefab;

    [SerializeField]
    protected bool hasInvulnerability = false;

    [SerializeField]
    protected bool generateParticlesOnDeath = false;

    [System.NonSerialized]
    public bool stunned = false;

    [System.NonSerialized]
    public bool invulnerable = false;

    private IEnumerator DamageRoutine()
    {

        this.stunned = true;

        for (int i = 0; i < 8; ++i)
        {
            SetVisible(true);

            yield return new WaitForSeconds(0.05f);

            SetVisible(false);

            yield return new WaitForSeconds(0.05f);

            SetVisible(true);

            if (i >= 2)
            {
                this.stunned = false;
                this.invulnerable = hasInvulnerability;
            }
        }

        this.invulnerable = false;
    }

    public void SetVisible(bool visible)
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


    protected Rigidbody2D CreateDeathParticle(float speed, Vector3 pos, Vector3 vel)
    {
        Rigidbody2D particle = (Rigidbody2D)Instantiate(deathParticlePrefab, pos, transform.rotation);
        particle.velocity = vel * speed;

        Destroy(particle.gameObject, 3.6f);

        return particle;
    }

    // 
    protected IEnumerator CreateDeathParticles(Vector3 pos)
    {
        float deathParticleSpeed = 48.0f;

        // Before the wait...
        Vector3 p1 = pos + Vector3.up;
        Vector3 p2 = pos - Vector3.up;
        Vector3 p3 = pos + Vector3.right;
        Vector3 p4 = pos - Vector3.right;

        Vector3 p5 = pos + Vector3.up + Vector3.right;
        Vector3 p6 = pos + Vector3.up - Vector3.right;
        Vector3 p7 = pos - Vector3.up - Vector3.right;
        Vector3 p8 = pos - Vector3.up + Vector3.right;

        p1.z = p2.z = -5;
        p3.z = p4.z = -7;
        p5.z = p6.z = p7.z = p8.z = -9;

        this.CreateDeathParticle(deathParticleSpeed, p1, (transform.up));
        this.CreateDeathParticle(deathParticleSpeed, p2, (-transform.up));
        this.CreateDeathParticle(deathParticleSpeed, p3, (transform.right));
        this.CreateDeathParticle(deathParticleSpeed, p4, (-transform.right));
        this.CreateDeathParticle(deathParticleSpeed, p5, (transform.up + transform.right));
        this.CreateDeathParticle(deathParticleSpeed, p6, (transform.up - transform.right));
        this.CreateDeathParticle(deathParticleSpeed, p7, (-transform.up - transform.right));
        this.CreateDeathParticle(deathParticleSpeed, p8, (-transform.up + transform.right));

        // Start the wait...
        yield return new WaitForSeconds(0.7f);

        // After the wait...
        this.CreateDeathParticle(deathParticleSpeed / 2.5f, p1, transform.up);
        this.CreateDeathParticle(deathParticleSpeed / 2.5f, p2, -transform.up);
        this.CreateDeathParticle(deathParticleSpeed / 2.5f, p3, transform.right);
        this.CreateDeathParticle(deathParticleSpeed / 2.5f, p4, -transform.right);
    }

    public void Kill()
    {
        GetComponent<Health>().ChangeHealth(-(GetComponent<Health>().CurrentHealth));
        SoundManager.Instance.PlaySingle(deathSoundEffect);
        StopAllCoroutines();
        SetVisible(false);
    }

    public void TakeDamage(float damage)
    {
        GetComponent<Health>().ChangeHealth(-damage);
        SoundManager.Instance.PlaySingle(damageSoundEffect);
        StartCoroutine(DamageRoutine());
    }

    public bool SurvivesDamage(float damage)
    {
        float health = GetComponent<Health>().CurrentHealth - damage;

        bool survives = true;

        if (health <= 0)
        {
            survives = false;
        }

        return survives;
    }


    public void KillCharacter()
    {
        var player = GetComponent<Player>();

        if (!player.GetComponent<Health>().IsDead)
        {
            Kill();

            if (this.generateParticlesOnDeath)
            {
                StartCoroutine(CreateDeathParticles(transform.position));
            }

            player.OnAfterDead();
        }
    }

    // Update is called once per frame
    void Update () {
        var player = GetComponent<Player>();

        var verticalExtent = Camera.main.orthographicSize;

        var levelCameraComponent = Camera.main.GetComponent<LevelCamera>();

        if (player.transform.position.y <
            Camera.main.transform.position.y - verticalExtent &&
            !player.GetComponent<Health>().IsDead &&
            !player.isInPassage &&
            !levelCameraComponent.switching)
        {
            KillCharacter();
        }

    }
}
