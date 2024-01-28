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
    private float slideSpeed = 5f;
    private float tolerance;
    private float freezeDelay = 5f; // Change this to increase or decrease duration of freeze condition.
    private float freezeTimer;
    public bool IsFrozen => tolerance <= 0;
    
    // Object References
    protected GameObject playerRef;
    private EnemyHandler handler;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    [SerializeField] protected ParticleSystem vfx;

    protected void Start()
    {
        tolerance = maxHealth;
        SetReferences();
    }

    private void Update()
    {
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
                vfx.Stop();
                // Mechanical
                tolerance = maxHealth;
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

            // Freezes the enemy if their health is below zero
            if (IsFrozen) 
            {
                freezeTimer = freezeDelay;

                /// Freezing Logic
                // Visuals
                spriteRenderer.color = frozenColor;
                anim.StartPlayback();
                vfx.Stop();
                // Mechanical
                rb.velocity = Vector3.zero;
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
            rb.velocity = (playerRef.transform.position - transform.position).normalized * 5f;
    }

    private void Die() 
    {
        // TODO:: Drop EXP
        Destroy(gameObject);
    }
}
