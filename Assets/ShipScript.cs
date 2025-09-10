using UnityEngine;
using UnityEngine.InputSystem;

public class ShipScript : MonoBehaviour
{

    public float speed = 5f;
    public float bulletSpeed = 10f;
    public int startingLives = 3;
    private int lives;
    public GameObject bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lives = startingLives;
    }

    void FireBullet()
    {
        if (bullet != null)
        {
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

    // Update is called once per frame
    void Update()
    {
        float move = 0f;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            move -= 1f;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            move += 1f;

        Vector3 movement = new Vector3(move, 0, 0) * speed * Time.deltaTime;
        transform.Translate(movement);

        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FireBullet();
        }
    }

    public void LoseLife()
    {
        lives -= 1;
        var gameManager = FindFirstObjectByType<GameManagerScript>();
        if (gameManager != null)
        {
            gameManager.LoseLife(lives);
        }
        if (lives <= 0)
        {
            // Destroy self
            Destroy(gameObject);
        }
    }
}
