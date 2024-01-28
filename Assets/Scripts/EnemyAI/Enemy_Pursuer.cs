using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pursuer : Enemy_Base
{
    /// <summary>
    /// Called every frame. 
    /// Moves the enemy toward the player, attacking on a delay if they are in range.
    /// </summary>
    protected override void UnfrozenBehavior()
    {
        Vector2 playerOffset = playerRef.transform.position - transform.position;

        if (playerOffset.magnitude <= attackRange)
        {
            rb.velocity = Vector2.zero;

            if (attackTimer <= 0)
            {
                // TODO:: Deal damage to the player.
                Debug.Log(gameObject.name + ": Attacking");
                attackTimer = attackDelay;
            }
        }
        else 
        {
            rb.velocity = (playerOffset).normalized * moveSpeed;
        }
    }
}
