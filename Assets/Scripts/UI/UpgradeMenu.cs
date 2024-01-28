using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

// Number of offered upgrades fixed at 3
public class UpgradeMenu : MonoBehaviour
{
	// pool of upgrades available to the player
	public PlayerUpgrade[] upgradePool;

	private GameObject playerRef;

	// upgradePool indicies for upgrades offered by the menu buttons
	private PlayerUpgrade[] offeredUpgrades;

	void Start()
	{
		playerRef = GameObject.FindGameObjectWithTag("Player");
		offeredUpgrades = new PlayerUpgrade[3];
	}

	// parameter is the index of the button pressed by the player	
	public void GrantPlayerUpgrade(int offeredUpgradeIndex)
	{
		playerRef.GetComponent<PlayerAttackUser>().ReceiveUpgrade(offeredUpgrades[offeredUpgradeIndex]);
	}

	// called when player levels up
	public void PresentNewUpgrade()
	{
		int[] chosenIndicies = new int[] {-1, -1, -1};
		// Pick 3 random indicies of upgradePool
		// set ui elements using their data
		List<int> poolOptions = Enumerable.Range(0, upgradePool.Length).ToList();
		for(int i = 0; i < 2; i++)
		{
			int choice = Random.Range(0, poolOptions.Count());
			chosenIndicies[i] = poolOptions[choice];
			poolOptions.RemoveAt(choice);
		}

		for (int i = 0; i < 2; i++)
		{
			int chosenPoolIndex = chosenIndicies[i];
			SetUpgradeButton(i, upgradePool[chosenPoolIndex]);
			offeredUpgrades[i] = upgradePool[chosenPoolIndex];
		}
	}

	private void SetUpgradeButton(int buttonIndex, PlayerUpgrade upgrade)
	{
		var button = gameObject.transform.GetChild(0).GetChild(buttonIndex);
		TextMeshPro buttonNameText = button.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>();
		TextMeshPro buttonDescText = button.gameObject.transform.GetChild(1).GetComponent<TextMeshPro>();
		
		buttonNameText.text = upgrade.displayName;
		buttonDescText.text =  upgrade.description;
	}

}
