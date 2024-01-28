using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	
	public float speed;
	public int health;

	private int exp;
	private int level;
	private int nextLevelRequirement; // determines exp required to reach the next level

	private Rigidbody2D rb;
	private Animator anim;
	private UpgradeMenu upgradeMenu;
	private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		upgradeMenu = GameObject.FindGameObjectWithTag("UpgradeMenu").GetComponent<UpgradeMenu>();
    }

    // Update is called once per frame
    void Update()
    {
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);

        Vector2 moveInput = new Vector2(horizontal, vertical);
		moveVelocity = moveInput.normalized * speed;

		// added for testing level ups
		if (Input.GetKeyDown("h"))
		{
			LevelUp();
		}
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

	public void TakeDamage(int amount)
	{
		health -= amount;
		if (health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		Destroy(gameObject);
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
