using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Slow Aura Attack", menuName = "PlayerAttack/Slow Aura Attack")]

public class SlowAuraPlayerAttack : PlayerAttack
{
	// cooldown inherited from parent class
	public float radius;
	public float rateOfSlow;
	public int damage; // likely going to be 0
}
