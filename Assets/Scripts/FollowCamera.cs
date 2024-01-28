using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	private GameObject playerRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 newpos = new Vector3(playerRef.transform.position.x, playerRef.transform.position.y, -30f);
        transform.position = newpos;
    }
}
