using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "PlayerUpgrade")]

// represents an option made available to the player upon level up
// includes first-level attacks as well as higher tier ones
public class PlayerUpgrade : ScriptableObject
{
	// This MUST match the name of the attack that is to be upgraded
	// e.g. if this is a tier 2 upgrade for CleavePlayerAttack named "cleaveAttack",
	// both upgradeName and attackName must be "cleaveAttack"
	public string attackName;

	// Name displayed in the upgrade menu
	public string displayName;

	[TextArea(4,8)]
	public string description;

	// 1 = lowest tier of attack
	public int tier;

	// PlayerAttack that will be granted to the player upon receiving the upgrade
	public PlayerAttack playerAttack;
}
