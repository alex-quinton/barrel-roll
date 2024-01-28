using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zoner : Enemy_Base
{
    [Tooltip("The minimum and maximum distance away this enemy hover around the player.")]
    [SerializeField] protected Vector2 hoverRangeBand;
    [Tooltip("How fast the enemy swoops at the player.")]
    [SerializeField] protected float swoopSpeed;
    [Tooltip("How much time the enemy swoops for.")]
    [SerializeField] protected float swoopLength;
    private float swoopTimer;
    private bool didAttack = false;
    private bool hoverDir;

    /// <summary>
    /// Called every frame.
    /// Moves into the attack range of the player, swoops at the player to attack, then retreats to attack range to circle the player until.
    /// </summary>
    protected override void UnfrozenBehavior()
    {
        Vector2 playerOffset = playerRef.transform.position - transform.position;

        // Swooping logic
        if (swoopTimer > 0)
        {
            swoopTimer -= Time.deltaTime;
            
            if (playerOffset.magnitude <= attackRange && !didAttack)
            {
                // TODO:: Deal damage to the player
                Debug.Log(gameObject.name + ": Swoop attack");

                didAttack = true;
            }
            return; // End the logic
        }

        // Not swooping logic
        if (playerOffset.magnitude > hoverRangeBand.y)
        {
            rb.velocity = playerOffset.normalized * moveSpeed;
        }
        else if (playerOffset.magnitude < hoverRangeBand.x)
        {
            rb.velocity = -playerOffset.normalized * moveSpeed;
        }
        else
        {
            if (attackTimer <= 0) // Swoop the player if the timer is up
            {
                hoverDir = UnityEngine.Random.Range(0, 2) > .5f;
                attackTimer = attackDelay;
                swoopTimer = swoopLength;
                rb.velocity = playerOffset.normalized * swoopSpeed;
                Debug.Log(gameObject.name + ": Start swoop");
            }
            else // Circle then otherwise
            {
                rb.velocity = new Vector2(-playerOffset.y, playerOffset.x).normalized * (hoverDir ? -1 : 1);
            }
        }
    }
}
