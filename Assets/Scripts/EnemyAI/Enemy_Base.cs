using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy_Base : MonoBehaviour
{
    [Tooltip("How fast this enemy moves in units per second")]
    [SerializeField] protected float moveSpeed;
    [Tooltip("How much damage this enemy inflicts on attack.")]
    [SerializeField] protected int attackDamage;
    [Tooltip("How close this enemy must be to deal damage to the player.")]
    [SerializeField] protected float attackRange;
    [Tooltip("How often this enemy attacks the player.")]
    [SerializeField] protected float attackDelay;
    protected float attackTimer;
    protected float freezeTimer;
    protected GameObject playerRef;
    protected Rigidbody2D rb;
    public bool IsFrozen => freezeTimer > 0;

    protected void Start()
    {
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
            // TODO:: Set sprite to frozen

            freezeTimer -= Time.deltaTime;
        }
        else
        {

            // TODO:: Set sprite to unfrozen

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
    /// Used by the player to freeze this 
    /// </summary>
    /// <param name="freezeTime"></param>
    public void Freeze(float freezeTime) 
    {
        freezeTimer = freezeTime;
    }

    /// <summary>
    /// Describes the specific behavior of enemies while unfrozen.
    /// </summary>
    protected abstract void UnfrozenBehavior();
}
