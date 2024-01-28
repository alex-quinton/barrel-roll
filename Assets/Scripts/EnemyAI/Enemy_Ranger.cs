using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranger : Enemy_Base
{
    /// <summary>
    /// Called every frame.
    /// Moves into attack range of the player, then fires a projectile on a delay.
    /// </summary>
    protected override void UnfrozenBehavior()
    {
        Vector2 playerOffset = playerRef.transform.position - transform.position;

        if (playerOffset.magnitude <= attackRange)
        {
            rb.velocity = Vector2.zero;

            if (attackTimer <= 0)
            {
                // TODO:: Spawn projectile
                Debug.Log(gameObject.name + ": Spawning projectile");
                attackTimer = attackDelay;
            }
        }
        else
        {
            rb.velocity = (playerOffset).normalized * moveSpeed;
        }
    }
}
