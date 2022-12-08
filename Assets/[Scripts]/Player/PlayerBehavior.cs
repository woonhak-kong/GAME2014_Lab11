using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerBehavior : MonoBehaviour
{
    [Header("Player Movement")]
    public float horizontalForce;
    public float horizontalSpeed;
    public float verticalForce;
    public float airFactor;
    public Transform groundPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGrounded;

    [Header("Health System")]
    public HealthBarController health;
    public LifeCounterController lifeCounter;
    public DeathPlaneController deathPlane;


    [Header("Controller")]
    public Joystick leftJoystick;
    [Range(0.1f, 1.0f)]
    public float horizontalSensitivity;
    [Range(0.1f, 1.0f)]
    public float verticalSensitivity;

    [Header("Animations")]
    public Animator animator;
    public PlayerAnimationState playerAnimationState;

    [Header("Dust Trail")]
    public ParticleSystem dustTrail;
    public Color dustTrailColor;

    [Header("Screen Shake Properties")]
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float shakeIntensity;
    public float shakeDuration;
    public float shakeTimer;
    public bool isCameraShaking;

    private Rigidbody2D rigidbody;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        leftJoystick = GameObject.FindObjectOfType<Joystick>();
        health = FindObjectOfType<PlayerHealth>().GetComponent<HealthBarController>();
        lifeCounter = FindObjectOfType<LifeCounterController>();
        deathPlane = FindObjectOfType<DeathPlaneController>();
        soundManager = FindObjectOfType<SoundManager>();
        dustTrail = GetComponentInChildren<ParticleSystem>();


        isCameraShaking = false;
        shakeTimer = shakeDuration;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (health.value <= 0)
        {
            lifeCounter.LoseLife();
            if (lifeCounter.value > 0)
            {
                health.ResetHealth();
                deathPlane.Respawn(gameObject);
                soundManager.PlaySoundFX(SoundFX.DEATH, Channel.DEATH);
                // play the death sound
            }
        }

        if (lifeCounter.value <= 0)
        {
            SceneManager.LoadScene("End");
            // load the "End" Scene
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);
        isGrounded = hit;
        
        Move();
        Jump();
        AirCheck();

        // Camera Shake Control
        if (isCameraShaking)
        {
            shakeTimer -= Time.fixedDeltaTime;
            if (shakeTimer <= 0.0f)
            {
                perlin.m_AmplitudeGain = 0.0f;
                shakeTimer = shakeDuration;
                isCameraShaking = false;
            }
        }
    }

    private void Move()
    {
        float joysticHorizontal = leftJoystick != null ? leftJoystick.Horizontal : 0.0f;

        float x = Input.GetAxisRaw("Horizontal") + joysticHorizontal * horizontalSensitivity;
        if (x != 0.0f)
        {
            x = x > 0.0f ? 1.0f : -1.0f;
        }
        rigidbody.AddForce(Vector2.right * x * horizontalForce * (isGrounded ? 1 : airFactor));
        Vector2 newVelocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -horizontalSpeed, horizontalSpeed), rigidbody.velocity.y);
        rigidbody.velocity = newVelocity;

        Flip(x);

        ChangeAnimation(PlayerAnimationState.RUN);

        if (isGrounded)
        {
            CreateDustrail();
        }

        if ( isGrounded && x == 0.0f)
        {
            ChangeAnimation(PlayerAnimationState.IDLE);
        }
    }

    private void CreateDustrail()
    {
        dustTrail.GetComponent<Renderer>().material.SetColor("_Color", dustTrailColor);
        dustTrail.Play();
    }

    private void ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeIntensity;
        isCameraShaking= true;
    }

    private void AirCheck()
    {
        if (!isGrounded)
        {
            ChangeAnimation(PlayerAnimationState.JUMP);
        }
    }

    private void Flip(float x)
    {
        if (x != 0.0f)
        {
            bool flip = false;
            if (x < 0.0f)
            {
                flip = true;
            }
            GetComponent<SpriteRenderer>().flipX = flip;
        }
    }
   
    private void Jump()
    {
        float joysticVertical = leftJoystick != null ? leftJoystick.Vertical : 0.0f;

        float y = Input.GetAxis("Jump") + joysticVertical;
        if (isGrounded && y > verticalSensitivity)
        {
            rigidbody.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            soundManager.PlaySoundFX(SoundFX.JUMP, Channel.JUMP);
        }
    }

    private void ChangeAnimation(PlayerAnimationState animationState)
    {
        playerAnimationState = animationState;
        animator.SetInteger("AnimationState", (int)playerAnimationState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(20);
            if (lifeCounter.value > 0)
            {
                soundManager.PlaySoundFX(SoundFX.HIT, Channel.HIT);
                ShakeCamera();
                // todo : play the "hurt" sound
            }
            // todo: updpate life value
            // todo: animation
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");
        if (other.gameObject.CompareTag("Hazard"))
        {
            health.TakeDamage(30);
            if (lifeCounter.value > 0)
            {
                soundManager.PlaySoundFX(SoundFX.HIT, Channel.HIT);
                ShakeCamera();
                // todo : play the "hurt" sound
            }
            // todo: updpate life value
            // todo: animation
        }
    }
  
}
