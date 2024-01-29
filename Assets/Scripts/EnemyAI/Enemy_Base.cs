using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Enemy_Base : MonoBehaviour
{
    // Combat
    [Tooltip("How much damage the enemy can take before freezing.")]
    [SerializeField] protected float maxHealth;
    [Tooltip("How fast this enemy moves in units per second.")]
    [SerializeField] protected float moveSpeed;
    [Tooltip("How much damage this enemy inflicts on attack.")]
    [SerializeField] protected int attackDamage;
    [Tooltip("How close this enemy must be to deal damage to the player.")]
    [SerializeField] protected float attackRange;
    [Tooltip("How often this enemy attacks the player.")]
    [SerializeField] protected float attackDelay;
    protected float attackTimer;

    // Freeze
    protected Color normalColor = new Color(255, 255, 255); // White
    protected Color frozenColor = new Color(0, 75, 255); // Blue Freeze
    private float slideSpeed = 4f;
    private float tolerance;
    private float freezeDelay = 5f; // Change this to increase or decrease duration of freeze condition.
    private float freezeTimer;
    public bool IsFrozen => tolerance <= 0;
    
    // Object References
    protected GameObject playerRef;
    private EnemyHandler handler;
    protected Rigidbody2D rb;
    protected Animator anim;
    [SerializeField] protected Collider2D col;
    [SerializeField] protected Collider2D trig;
    protected SpriteRenderer spriteRenderer;

	private bool doWallCollide = false;

	private AudioSource audioSource;
	public AudioClip[] sounds;

    [SerializeField] protected ParticleSystem vfx;

    protected void Start()
    {
        tolerance = maxHealth;
        SetReferences();
    }

    private void Update()
    {
		if (!doWallCollide)
		{
			bool assumeDoesCollide = true;
			foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 0.3f))
			{
				if (collider.gameObject.tag == "Wall")
				{
					//Debug.Log("hit wall");
					assumeDoesCollide = false;
				}
			}

			if (assumeDoesCollide) 
			{
				//Debug.Log("Changing excludeLayers");
				col.excludeLayers = col.excludeLayers &~ (byte) (1 << 3);
				doWallCollide = true;
			}
		}
        // Update the attack timer
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // Handle the enemy's frozen state
        if (IsFrozen) 
        {
            freezeTimer -= Time.deltaTime;

            // Freeze delay is up, revive the enemy.
            if (freezeTimer <= 0) 
            {
                /// Unfreeze Logic
                // Visuals
                spriteRenderer.color = normalColor;
                anim.StopPlayback();
				if (vfx)
                	vfx.Stop();
                // Mechanical
                tolerance = maxHealth;
                if (col)
                    col.enabled = true;
                if (trig)
                    trig.enabled = false;
            }
        }
        else
        {
            handler?.CheckRelocate(gameObject);

            UnfrozenBehavior();
        }
    }

    /// <summary>
    /// Sets all necessary object references.
    /// </summary>
    private void SetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRef = GameObject.FindGameObjectWithTag("Player");

		audioSource = GetComponent<AudioSource>();

        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        if (gc)
            handler = gc.GetComponent<EnemyHandler>();
    }

    /// <summary>
    /// Used by the player to apply damage to this enemy.
    /// Will freeze the enemy if their tolerance drops below 0.
    /// </summary>
    /// <param name="incomingDamage">The amount of damage to apply.</param>
    public void ApplyDamage(float incomingDamage) 
    {
        if (tolerance > 0) 
        {
            tolerance -= incomingDamage;

			//Debug.Log("new tolerance: " + tolerance);
            // Freezes the enemy if their health is below zero
            if (IsFrozen) 
            {
                freezeTimer = freezeDelay;

                /// Freezing Logic
                // Visuals
                spriteRenderer.color = frozenColor;
                anim.StartPlayback();
				if (vfx)
                	vfx.Stop();
                // Mechanical
                rb.velocity = Vector3.zero;
				Debug.Log("Set rb velocity: " + rb.velocity);
                if (col)
                    col.enabled = false;
                if (trig)
                    trig.enabled = true;
            }else{
				//Debug.Log("Not frozen! Tolerance: " + tolerance);
			}
        }
    }

    /// <summary>
    /// Describes the specific behavior of enemies while unfrozen.
    /// </summary>
    protected abstract void UnfrozenBehavior();

    /// <summary>
    /// Handles frozen logic for enemies on colliding with other objects.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsFrozen) 
        {
            bool isMoving = rb.velocity.magnitude > 0;
            switch (collision.gameObject.tag)
            {
                case "Player":
                    Push();
                    break;
                case "Enemy":
                    if (isMoving)
						//Debug.Log("Applying frozen collision damage");
                        collision.gameObject.GetComponent<Enemy_Base>().ApplyDamage(50);
                    break;
                case "Wall":
                    Die();
                    break;
            }
        }
    }

    public void Push() 
    {
        if (IsFrozen && rb.velocity.magnitude <= 0)
            rb.velocity = (transform.position - playerRef.transform.position).normalized * slideSpeed;
    }

    private void Die() 
    {
        // TODO:: Drop EXP
        Destroy(gameObject);
		PlayAudio(0);
    }

	public void PlayAudio(int soundIndex)
	{
		audioSource.PlayOneShot(sounds[soundIndex], 0.5f);
	}
}
