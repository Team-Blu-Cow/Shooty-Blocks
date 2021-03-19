using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>(); // Get rigidbody of game object
        rb.velocity = new Vector2(0.0f, 10.0f); // Set the velocity to travel up 10 units in the y-axis every second
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > 6) // If the bullet has went off screen
        {
            Destroy(gameObject); // Delete it
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy") // If the bullet has hit an enemy
        {
            collision.gameObject.GetComponentInParent<Block>().Damage(player.GetComponent<PlayerController>().firePower); // Damage the block by the player's firing power
            Destroy(gameObject); // Destroy the bullet
        }        
    }
}
