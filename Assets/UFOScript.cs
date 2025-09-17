using UnityEngine;

public class UFOScript : MonoBehaviour
{
    public float speed = 6f;           // UFO movement speed
    public float leftLimit = -10f;     // Left boundary (world X position)
    public float rightLimit = 10f;     // Right boundary (world X position)
    public bool moveRight = true;      // Direction: true = right, false = left

    void Update()
    {
        // Move UFO horizontally
        float dir = moveRight ? 1f : -1f;
        transform.position += Vector3.right * dir * speed * Time.deltaTime;

        // Destroy UFO if it goes out of bounds
        if (transform.position.x < leftLimit || transform.position.x > rightLimit)
        {
            Destroy(gameObject);
        }
    }
}
