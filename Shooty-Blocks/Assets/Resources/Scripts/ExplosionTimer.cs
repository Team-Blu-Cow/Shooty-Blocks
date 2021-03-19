using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTimer : MonoBehaviour
{

    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // Add delta time to timer, to time how long the particle effect has stayed alive for
        if(timer > 1.0f) // If 1 second has passed
        {
            Destroy(gameObject); // Destroy the particle system
        }
    }
}
