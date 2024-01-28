using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Number of offered upgrades fixed at 3
public class UpgradeMenu : MonoBehaviour
{
	// pool of upgrades available to the player
	public PlayerUpgrade[] upgradePool;

	// called when player levels up
	public void NewUpgrade()
	{
		// Pick 3 indicies of upgradePool
		// set ui elements using their data
	}
}