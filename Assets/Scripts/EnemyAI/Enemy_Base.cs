using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
    private float tolerance;
    private float freezeDelay = 5f; // Change this to increase or decrease duration of freeze condition.
    private float freezeTimer;
    public bool IsFrozen => tolerance <= 0;
    
    // Object References
    protected GameObject playerRef;
    protected Rigidbody2D rb;

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
                tolerance = maxHealth;

                // TODO:: Set sprite to unfrozen
            }
        }
        else
        {
            UnfrozenBehavior();
        }
    }

    /// <summary>
    /// Sets all necessary object references.
    /// </summary>
    private void SetReferences()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
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
                Freeze();

                // TODO:: Set sprite to frozen
            }
        }
    }

    /// <summary>
    /// Freezes the enemy for a set period of time
    /// </summary>
    /// <param name="freezeTime"></param>
    private void Freeze() 
    {
        Freeze(freezeDelay);
    }
    private void Freeze(float freezeTime) 
    {
        freezeTimer = freezeTime;
    }

    /// <summary>
    /// Describes the specific behavior of enemies while unfrozen.
    /// </summary>
    protected abstract void UnfrozenBehavior();
}
