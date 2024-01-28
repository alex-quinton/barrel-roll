using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	
	public float speed;

	private int exp;
	private int level;
	private int nextLevelRequirement; // determines exp required to reach the next level

	private Rigidbody2D rb;
	private UpgradeMenu upgradeMenu;
	private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu").GetComponent<UpgradeMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		moveVelocity = moveInput.normalized * speed;
    }

	void FixedUpdate()
	{
		rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
	}

	public void GainExp(int amount)
	{
		exp += amount;
		if (exp >= nextLevelRequirement)
		{
			LevelUp();
		}
	}

	// exp is not set to 0 when levelling up
	private void LevelUp()
	{
		updateLevelRequirement();
		upgradeMenu.PresentNewUpgrade();
	}

	// sets the level requirement to the amount needed for the next level
	// uses runescape calculation as placeholder
	private void updateLevelRequirement()
	{
		nextLevelRequirement = (int) Mathf.Floor( 0.25f * Mathf.Floor( level + 300*Mathf.Pow(2, level/7) ) );
	}

	// code for receiving upgrades is in PlayerAttackUser
}
