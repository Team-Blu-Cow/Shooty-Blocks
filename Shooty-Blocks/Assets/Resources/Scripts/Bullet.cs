using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > 6)
        {
            Destroy(gameObject);
        }
    }
}
