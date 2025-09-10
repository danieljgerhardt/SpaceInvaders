using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public AudioClip bulletSound;
    public AudioClip alienDeathSound;
    public AudioClip playerHitSound;
    public AudioClip shieldHitSound;
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);           // Destroy the bullet

        if (collision.gameObject.CompareTag("Alien"))
        {
            Destroy(collision.gameObject); // Destroy the alien
            var gameManagerScript = FindFirstObjectByType<GameManagerScript>(); 
            if (gameManagerScript != null)
            {
                gameManagerScript.IncreaseScore();
            }
            AudioSource.PlayClipAtPoint(alienDeathSound, transform.position);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            var shipScript = collision.gameObject.GetComponent<ShipScript>();
            if (shipScript != null)
            {
                shipScript.LoseLife();
            }
            AudioSource.PlayClipAtPoint(playerHitSound, transform.position);
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(collision.gameObject); // Destroy the shield
            AudioSource.PlayClipAtPoint(shieldHitSound, transform.position);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource.PlayClipAtPoint(bulletSound, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
