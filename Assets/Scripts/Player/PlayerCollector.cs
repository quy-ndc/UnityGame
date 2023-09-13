using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerCollector.radius = player.currentMagnet;    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
        {
            // Pulling animation when player pick up items

            // Get Rigridbody of item
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            // Vector2 pointing from player to item
            Vector2 forceDirection = (transform.position - collision.transform.position).normalized;
            // Apply force to item 
            rb.AddForce(forceDirection * pullSpeed);

            collectible.Collect();
        }    
    }
}
