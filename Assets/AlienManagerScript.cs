using System.Collections.Generic;
using UnityEngine;
public class AlienManagerScript : MonoBehaviour
{
    public float shootInterval = 2f;
    public GameObject bulletPrefab;
    private float shootTimer = 0f;

    public float speedIncreaseOnDrop = 0.15f;

    public float forwardStep = 0.5f;
    public float lossZ = -8.3f; // Z position at which player loses
    
    private float leftLimit = -5f;
    private float rightLimit = 5f;

    public float ultimateTimerDecreaseAmount = -0.1f;

    // UFO spawning fields
    public GameObject ufoPrefab;
    public float minUFOSpawnInterval = 15f;
    public float maxUFOSpawnInterval = 30f;
    private float ufoSpawnTimer = 0f;
    private float nextUFOSpawnTime = 0f;

    private float direction;

    private void Start()
    {
        float leftMostAlien = float.MaxValue;
        float rightMostAlien = float.MinValue;
        foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
        {
            if (alien.transform.position.x < leftMostAlien)
            {
                leftMostAlien = alien.transform.position.x;
                leftLimit = leftMostAlien - alien.moveDistance * 0.95f;
            }
            if (alien.transform.position.x > rightMostAlien)
            {
                rightMostAlien = alien.transform.position.x;
                rightLimit = alien.transform.position.x + alien.moveDistance * 0.95f;
            }
        }

        // Set initial UFO spawn time
        nextUFOSpawnTime = Random.Range(minUFOSpawnInterval, maxUFOSpawnInterval);

        direction = 1;
    }

    void Update()
    {
        // If all aliens hit, win the game
        AlienScript[] aliens = FindObjectsByType<AlienScript>(0);
        bool allHit = aliens.Length > 0;
        foreach (AlienScript alien in aliens)
        {
            if (!alien.getIsHit())
            {
                allHit = false;
                break;
            }
        }
        if (allHit)
        {
            var gameManager = FindFirstObjectByType<GameManagerScript>();
            if (gameManager != null)
            {
                gameManager.SetWinStatusToWin();
            }
        }

        // Check if any alien has hit a left or right bound
        bool hitBoundary = false;
        foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
        {
            if (alien.getIsHit()) continue; // Ignore hit aliens
            if ((direction == 1 && alien.transform.position.x >= rightLimit) ||
                (direction == -1 && alien.transform.position.x <= leftLimit))
            {
                hitBoundary = true;
                direction *= -1;
                break;
            }
        }

        // Move all aliens forward if boundary hit
        if (hitBoundary)
        {
            foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
            {
                if (alien.getIsHit()) continue; // Ignore hit aliens
                alien.transform.position += new Vector3(0, 0, -forwardStep);
                alien.direction *= -1;
                alien.speed += speedIncreaseOnDrop;
            }
        }

        foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
        {
            if (alien.getIsHit()) continue; // Ignore hit aliens
            if (alien.transform.position.z <= lossZ)
            {
                var gameManager = FindFirstObjectByType<GameManagerScript>();
                if (gameManager != null)
                {
                    gameManager.SetWinStatusToLose();
                }
                break; // Only need to trigger once
            }
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootFromLowestAliens();
        }

        // UFO spawn logic
        ufoSpawnTimer += Time.deltaTime;
        if (ufoPrefab != null && ufoSpawnTimer >= nextUFOSpawnTime)
        {
            SpawnUFO();
            ufoSpawnTimer = 0f;
            nextUFOSpawnTime = Random.Range(minUFOSpawnInterval, maxUFOSpawnInterval);
        }
    }
    void SpawnUFO()
    {
        // Randomly choose left or right spawn
        bool moveRight = Random.value > 0.5f;
        float ufoLeft = leftLimit * 3.0f;
        float ufoRight = rightLimit * 3.0f;
        float spawnX = moveRight ? ufoLeft : ufoRight;
        float spawnY = 0.5f; // Adjust as needed for your scene
        float spawnZ = 11.4f;  // Adjust as needed for your scene
        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);
        GameObject ufo = Instantiate(ufoPrefab, spawnPos, Quaternion.identity);
        UFOScript ufoScript = ufo.GetComponent<UFOScript>();
        if (ufoScript != null)
        {
            ufoScript.moveRight = moveRight;
            // Optionally set limits if your UFO prefab doesn't have them set
            ufoScript.leftLimit = ufoLeft;
            ufoScript.rightLimit = ufoRight;
        }
    }
    void ShootFromLowestAliens()
    {
        // Group aliens by X position (column)
        Dictionary<float, AlienScript> lowestAliens = new Dictionary<float, AlienScript>();
        foreach (AlienScript alien in FindObjectsByType<AlienScript>(0))
        {
            if (alien.getIsHit()) continue; // Ignore hit aliens
            float x = alien.transform.position.x;
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

    public void LowerShootTimer()
    {
        shootInterval -= ultimateTimerDecreaseAmount;
    }
}
