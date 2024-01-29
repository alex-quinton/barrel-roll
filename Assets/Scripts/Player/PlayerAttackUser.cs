using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles logic for attack usage
// as well as gaining & upgrading attacks
public class PlayerAttackUser : MonoBehaviour
{
	// Array of PlayerAttacks currently equipped on the player
	// Each attack tracks its cooldown & is used when cooldown is ready
	public PlayerAttack[] attacks;

	// each cooldown corresponds to the attack at the same index in attacks array
	public int[] cooldowns;

	private Animator anim;

	[SerializeField]
	private Animator cleaveAnim;

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
		cleaveAnim.Play("playerCleaveAttack", -1, 0f);
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

	// adds an attack to attacks array if tier is 1 and space is available
	// otherwise replaces an existing attack with the upgraded version
	public void ReceiveUpgrade(PlayerUpgrade upgrade)
	{
		if (upgrade.tier == 1)
		{
			if (!HasEmptyAttackSlot())
			{
				Debug.Log("Warning! Attempting to equip tier 1 upgrade while player's attack slots are full.\nPlayer should not be offered T1 upgrades while attacks are full!");
				return;
			}

			int newAtkIdx = GetFirstEmptyAttackIndex();
			attacks[newAtkIdx] = upgrade.playerAttack;

			// free trigger for new attack
			cooldowns[newAtkIdx] = 1;
		}else
		{
			int idxToReplace = GetIdxOfMatchingAttackName(upgrade.attackName);
			if (idxToReplace == -1)
			{
				Debug.Log("Warning! Attempting to equip T2+ upgrade for an attack the player doesn't have.\nPlayer should not be offered T2+ upgrades for attacks they don't have");
				return;
			}

			attacks[idxToReplace] = upgrade.playerAttack;

			// free trigger for newly upgraded attack
			cooldowns[idxToReplace] = 1;
		}
	}

	private bool HasEmptyAttackSlot()
	{
		bool result = false;
		for (int i = 0; i < attacks.Length; i++)
		{
			if (attacks[i] == null)
				result = true;
		}

		return result;
	}

	private int GetFirstEmptyAttackIndex()
	{
		for (int i = 0; i < attacks.Length; i++)
		{
			if (attacks[i] == null)
				return i;
		}

		return -1;
	}

	private int GetIdxOfMatchingAttackName(string attackName)
	{
		for (int i = 0; i < attacks.Length; i++)
		{
			if (attacks[i].attackName == attackName)
				return i;
		}

		return -1;
	}
}