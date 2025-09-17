using System;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public AudioClip bulletSound;
    public AudioClip alienDeathSound;
    public AudioClip playerHitSound;
    public AudioClip shieldHitSound;

    private bool hasCollided = false;
    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        if (collision.gameObject.CompareTag("Alien"))
        {
            var alienScript = collision.gameObject.GetComponent<AlienScript>();
            alienScript.setHit();

            var gameManagerScript = FindFirstObjectByType<GameManagerScript>(); 
            if (gameManagerScript != null)
            {
                gameManagerScript.IncreaseScore();
            }
            AudioSource.PlayClipAtPoint(alienDeathSound, transform.position);
        }

        if (collision.gameObject.CompareTag("UFO"))
        {
            Destroy(collision.gameObject);

            var gameManagerScript = FindFirstObjectByType<GameManagerScript>();
            if (gameManagerScript != null)
            {
                gameManagerScript.IncreaseScore();
                gameManagerScript.IncreaseScore();
                gameManagerScript.IncreaseScore();
            }

            //get player and increase resources by 3
            var ship = FindFirstObjectByType<ShipScript>();
            if (ship != null)
            {
                ship.CollectResources(3);
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

        hasCollided = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource.PlayClipAtPoint(bulletSound, transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (hasCollided && rb != null)
        {
            //rb.linearVelocity += new Vector3(0, 0, -9.68f * Time.fixedDeltaTime);
            rb.AddForce(new Vector3(0, 0, -9.68f), ForceMode.Acceleration);
        }
    }
}
