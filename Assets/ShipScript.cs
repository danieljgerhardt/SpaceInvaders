using UnityEngine;
using UnityEngine.InputSystem;

public class ShipScript : MonoBehaviour
{

    public float speed = 8f;
    public float bulletSpeed = 10f;
    public int startingLives = 3;
    private int lives;
    private int resources = 30;
    public GameObject bullet;
    public AudioClip ultimateSound;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lives = startingLives;
        rb = GetComponent<Rigidbody>();
        var gameManager = FindFirstObjectByType<GameManagerScript>();
        if (gameManager != null)
        {
            gameManager.UpdateResources(resources);
        }
    }

    void FireBullet()
    {
        if (bullet != null && resources > 0)
        {
            resources--;
            var gameManager = FindFirstObjectByType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.UpdateResources(-1);
            }
            GameObject newBullet = Instantiate(
                bullet,
                transform.position + new Vector3(0.0f, 0.0f, 2.0f),
                Quaternion.identity
            );

            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(0.0f, 0.0f, bulletSpeed);
            }
        }
    }

    void UseUltimate()
    {
        if (resources >= 6)
        {
            resources -= 6;
            lives += 1;
            AudioSource.PlayClipAtPoint(ultimateSound, transform.position);
            var gameManager = FindFirstObjectByType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.UpdateResources(-6);
                gameManager.UpdateLives(lives); // Update lives display
            }
            // Fire 3 bullets in a spread pattern, offsetting their X position
            float bulletSpacing = 0.5f; // Adjust as needed based on bullet size
            for (int i = -1; i <= 1; i++)
            {
                if (bullet != null)
                {
                    Vector3 offset = new Vector3(i * bulletSpacing, 0.0f, 2.0f);
                    GameObject newBullet = Instantiate(
                        bullet,
                        transform.position + offset,
                        Quaternion.Euler(0, i * 15, 0) // Spread by 15 degrees
                    );
                    Rigidbody rb = newBullet.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 direction = Quaternion.Euler(0, i * 15, 0) * Vector3.forward;
                        rb.linearVelocity = direction * bulletSpeed;
                    }
                }
            }

            //lower shoot interval in alien manager script
            var alienManager = FindFirstObjectByType<AlienManagerScript>();
            if (alienManager != null)
            {
                alienManager.LowerShootTimer();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float move = 0f;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            move -= 1f;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            move += 1f;

        //Vector3 movement = new Vector3(move, 0, 0) * speed * Time.deltaTime;
        //transform.Translate(movement);
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(move * speed, 0, 0);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireBullet();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            UseUltimate();
        }
    }

    public void LoseLife()
    {
        lives -= 1;
        var gameManager = FindFirstObjectByType<GameManagerScript>();
        if (gameManager != null)
        {
            gameManager.UpdateLives(lives);
        }
        if (lives <= 0)
        {
            // Destroy self
            Destroy(gameObject);
        }
    }

    public void CollectResources(int amount)
    {
        resources += amount;
        var gameManager = FindFirstObjectByType<GameManagerScript>();
        if (gameManager != null)
        {
            gameManager.UpdateResources(amount);
        }
    }
}
