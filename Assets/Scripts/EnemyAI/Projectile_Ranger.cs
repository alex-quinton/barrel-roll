using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile_Ranger : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [System.NonSerialized] public int damage;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.velocity = -transform.right * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
                Destroy(gameObject);
                break;
            case "Wall":
                Destroy(gameObject);
                break;
        }
    }
}
