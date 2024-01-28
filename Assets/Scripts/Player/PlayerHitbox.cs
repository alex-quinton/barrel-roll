using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
	public int damage;

	private Camera cam;
	private Vector3 playerPos;
	private Vector2 mousePos;
	private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
		cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
	// Hitbox updates to face cursor each frame
    void Update()
    {

		playerPos = GameObject.FindWithTag("Player").transform.position;
		mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		Quaternion targetRotation = Quaternion.LookRotation(mousePos - new Vector2(playerPos.x, playerPos.y));

		targetRotation.eulerAngles += new Vector3(90, 0, 0);
		//Debug.Log("targetRotation: " + targetRotation.eulerAngles);

		targetRotation.x = 0.0f;
		targetRotation.y = 0.0f;
		GetComponent<Transform>().rotation = targetRotation;
    }
}
