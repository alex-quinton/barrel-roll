using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
	public bool isPlayerBullet;
	public int damage;
	public float speed;

	private int lifetime;
	private Camera cam; // used for getting cursor position
	private Rigidbody2D rb;
	private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
		cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		rb = GetComponent<Rigidbody2D>();

		Transform playerPos = GameObject.FindWithTag("Player").transform;
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		target = (mousePos - new Vector2(playerPos.position.x, playerPos.position.y)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime --;
		if (lifetime <= 0)
			Destroy(gameObject);
		
		rb.MovePosition(rb.position + (target * Time.fixedDeltaTime * speed ));
    }

	// used to set lifetime when object is instantiated
	// in order for the range to be determined by the PlayerAttack scriptable object
	public void setLifetime(int newLifetime)
	{
		//Debug.Log("setting lifetime: " + newLifetime);
		lifetime = newLifetime;
	}
}
