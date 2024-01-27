using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Attack", menuName = "PlayerAttack/Cleave Attack")]

public class CleavePlayerAttack : PlayerAttack
{
	// cooldown inherited from parent class
	public float width;
	public float length;
	public int damage;
	public float pushStrength;
}
