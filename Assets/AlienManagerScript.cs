using System.Collections.Generic;
using UnityEngine;
public class AlienManagerScript : MonoBehaviour
{
    public float shootInterval = 2f;
    public GameObject bulletPrefab;
    private float shootTimer = 0f;

    public float forwardStep = 0.5f;
    public float lossZ = -8.3f; // Z position at which player loses
    
    private float leftLimit = -5f;
    private float rightLimit = 5f;

    private void Start()
    {
        float leftMostAlien = float.MaxValue;
        float rightMostAlien = float.MinValue;
        foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
        {
            if (alien.transform.position.x < leftMostAlien)
            {
                leftMostAlien = alien.transform.position.x;
                leftLimit = leftMostAlien - alien.moveDistance * 0.99f;
            }
            if (alien.transform.position.x > rightMostAlien)
            {
                rightMostAlien = alien.transform.position.x;
                rightLimit = alien.transform.position.x + alien.moveDistance * 0.99f;
            }
        }
    }

    void Update()
    {
        int direction = 0;
        // If no aliens exist, win the game
        if (FindObjectsByType<AlienScript>(0).Length == 0)
        {
            var gameManager = FindFirstObjectByType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.SetWinStatusToWin();
            }
        } else
        {
            // Get direction of first alien
            AlienScript firstAlien = FindFirstObjectByType<AlienScript>();
            if (firstAlien != null)
            {
                direction = firstAlien.direction;
            }
        }

        // Check if any alien has hit a left or right bound
        bool hitBoundary = false;
        foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
        {
            if ((direction == 1 && alien.transform.position.x >= rightLimit) ||
                (direction == -1 && alien.transform.position.x <= leftLimit))
            {
                hitBoundary = true;
                break;
            }
        }

        // Move all aliens forward if boundary hit
        if (hitBoundary) {
            foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
            {
                alien.transform.position += new Vector3(0, 0, -forwardStep);
                alien.direction *= -1;

                // Check if any alien has reached the loss Z position
                if (alien.transform.position.z <= lossZ)
                {
                    var gameManager = FindFirstObjectByType<GameManagerScript>();
                    if (gameManager != null)
                    {
                        gameManager.SetWinStatusToLose();
                    }
                }
            }
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootFromLowestAliens();
        }
    }
    void ShootFromLowestAliens()
    {
        // Group aliens by X position (column)
        Dictionary<float, AlienScript> lowestAliens = new Dictionary<float, AlienScript>();
        foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
        {
            float x = Mathf.Round(alien.transform.position.x * 10f) / 10f; // Tolerance for float
            if (!lowestAliens.ContainsKey(x) || alien.transform.position.z < lowestAliens[x].transform.position.z)
            {
                lowestAliens[x] = alien;
            }
        }

        // Collect all lowest aliens into a list
        List<AlienScript> candidates = new List<AlienScript>(lowestAliens.Values);

        // Pick one at random to shoot
        if (candidates.Count > 0)
        {
            int idx = Random.Range(0, candidates.Count);
            candidates[idx].Shoot(bulletPrefab);
        }
    }
}
