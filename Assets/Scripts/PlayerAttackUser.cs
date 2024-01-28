using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillUser : MonoBehaviour
{
	// Array of PlayerAttacks currently equipped on the player
	// Each attack tracks its cooldown & is used when cooldown is ready
	public PlayerAttack[] attacks;

	// each cooldown corresponds to the attack at the same index in attacks array
	public int[] cooldowns;

	private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
		InitializeCooldowns();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cooldowns.Length; i++)
		{
			if (cooldowns[i] > 0)
				cooldowns[i] -= 1;

			if (cooldowns[i] == 0)
			{
				InvokeAttack(attacks[i]);
				cooldowns[i] = attacks[i].cooldown;
				//Debug.Log("setting cooldown to: " + attacks[i].cooldown);
				continue;
			}

		}
    }

//	private void InvokeAttack(PlayerAttack atk) => atk.GetType().ToString() switch
//	{
//		"RangedPlayerAttack" => PerformRangedAttack((RangedPlayerAttack)atk),
//		"CleavePlayerAttack" => PerformCleaveAttack((CleavePlayerAttack)atk),
//		"SlowAuraPlayerAttack" => PerformSlowAuraAttack((SlowAuraPlayerAttack)atk),
//		_ => Debug.Log("Warning: player attack class name has no case for InvokeAttack()!")
//	};

// TODO: fix animation states for attacks so that they dont interfere with each other
	private void InvokeAttack(PlayerAttack atk)
	{
		//Debug.Log("Invoking attack: " + atk.attackName);
		switch(atk.GetType().ToString())
		{
			case "RangedPlayerAttack":
				PerformRangedAttack((RangedPlayerAttack) atk);
				break;
			case "CleavePlayerAttack":
				PerformCleaveAttack((CleavePlayerAttack) atk);
				break;
			case "SlowAuraPlayerAttack":
				PerformSlowAuraAttack((SlowAuraPlayerAttack) atk);
				break;
			default:
				Debug.Log("Warning: player attack class name has no case for InvokeAttack()!");
				break;
		}
	}

	private void PerformSlowAuraAttack(SlowAuraPlayerAttack atk)
	{
		anim.Play("playerSlowAuraAttack", -1, 0f);
	}

	private void PerformRangedAttack(RangedPlayerAttack atk)
	{
		Vector3 spawnPosition = GetComponent<Transform>().position;
		GameObject newProjectile = Instantiate(atk.projectile);
		newProjectile.GetComponent<PlayerProjectile>().setLifetime(atk.range); // perform calculation for lifetime based on range/speed?
		newProjectile.GetComponent<Transform>().position = spawnPosition;
	}

	private void PerformCleaveAttack(CleavePlayerAttack atk)
	{
		//GameObject cleaveHitbox = gameObject.transform.GetChild(0).gameObject;
		anim.Play("playerCleaveAttack", -1, 0f);
	}

	private void InitializeCooldowns()
	{
	    for (int i = 0; i < attacks.Length; i++)
		{
			PlayerAttack atk = attacks[i];
			if (atk != null)
			{
				cooldowns[i] = atk.cooldown;
			}else
			{
				cooldowns[i] = -1;
			}
		}
	}
}