using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

    [SerializeField]
    protected LevelCamera levelCamera;

    [SerializeField]
    protected AudioClip deathSoundEffect;

    [SerializeField]
    protected Rigidbody2D deathParticlePrefab;

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

    protected IEnumerator WaitAndResetRoutine()
    {
        // Before the wait... 
        /*health.IsDead = true;
        movement.IsFrozen = true;
        playerTexRend.enabled = false;
        levelCamera.ShouldStayStill = true;
        shooting.CanShoot = false;


        CharacterController cc = (CharacterController)GetComponent(typeof(CharacterController));
        cc.detectCollisions = false;

        GameEngine.SoundManager.Stop(AirmanLevelSounds.STAGE);
        GameEngine.SoundManager.Stop(AirmanLevelSounds.BOSS_MUSIC);
        GameEngine.SoundManager.Play(AirmanLevelSounds.DEATH);*/

        // Start the wait... 

        GetComponent<Rigidbody2D>().simulated = false;

        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySingle(deathSoundEffect);

        yield return new WaitForSeconds(3.6f);

        FadeManager.Instance.Fade(true, 1.0f, OnDeathEndReset);
    }

    void OnDeathEndReset()
    {

        var player = GetComponent<Player>();

        var checkpointPosition = 
            CurrentGame.Instance.CurrentCheckpoint.transform.position;

        var cameraPosition = levelCamera.transform.position;

        player.transform.position = checkpointPosition;

        cameraPosition.x = checkpointPosition.x;
        cameraPosition.y = checkpointPosition.y;

        levelCamera.transform.position = cameraPosition;            

        GetComponent<Rigidbody2D>().simulated = true;

        FadeManager.Instance.Fade(false, 1.0f, null);

    }

    // Update is called once per frame
    void Update () {
        var player = GetComponent<Player>();

        var verticalExtent = levelCamera.GetComponent<Camera>().orthographicSize;

        if(player.transform.position.y < 
            levelCamera.transform.position.y - verticalExtent &&
            !player.GetComponent<Health>().IsDead &&
            !player.isInPassage &&
            !levelCamera.switching)
        {
            player.Kill();

            StartCoroutine(CreateDeathParticles(transform.position));

            if (GetComponent<HumanDetector>().IsHuman)
            {
                StartCoroutine(WaitAndResetRoutine());
            }
            
        }

    }
}
