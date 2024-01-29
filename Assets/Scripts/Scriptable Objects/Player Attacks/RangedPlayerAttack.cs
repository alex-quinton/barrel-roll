using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Attack", menuName = "PlayerAttack/Ranged Attack")]

public class RangedPlayerAttack : PlayerAttack
{
	// cooldown inherited from parent class
	
	// for now, range is directly used to set projectile's lifetime
	public float range; // int because it's used to set PlayerProjectile lifetime
	public int damage;
	public float pushStrength;

	public GameObject projectile;
}
