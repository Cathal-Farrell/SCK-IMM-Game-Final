using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Prefabs for all enemy types and powerups 
    public GameObject enemyPrefab;          
    public GameObject fastEnemyPrefab;
    public GameObject slowEnemyPrefab;
    public GameObject powerupSlow;
    public GameObject powerupInfAmmo;
    public GameObject powerupHeal;

    // Retrive player and its rigid body
    public GameObject player;
    private Rigidbody playerRb;

    // Create array to store currently alive copies of each enemy type
    private GameObject[] enemies;
    private GameObject[] slowEnemies;
    private GameObject[] fastEnemies;  
    
    // Create array to store powerup types to be randomly chosen to spawn on every 3 rounds
    private GameObject[] powerups;

    // Enemies speed
    public static float enemySpeed = 5000f; 

    // Store x/y values to each edge of game area
    private float spawnRange = 42;      // Distance in each direction
    private float platformOffsetX = 0;  // Offset from x=0
    private float platformOffsetZ = 0;  // Offset from y=0

    // Quantity of each enemy type to be spawned per round
    private float enemiesToSpawn;       // Normal enemies
    private float fastEnemiesToSpawn;   // Fast enemies
    private float slowEnemiesToSpawn;   // Slow enemies

    // Current wave number
    private int waveNumber = 1;

    // Position enemy will be spawned at
    private float spawnPosX;
    private float spawnPosZ;

    // Current player position for creating "safety bubble" where enemies can't spawn
    private float playerPosX;
    private float playerPosZ;

    // Spawn cushion radius, "Safety bubble" radius
    private float spawnCushionRadius = 10f;

    void Start()
    {
        // Retrieve rigid body
        playerRb = player.GetComponent<Rigidbody>();
        // Create array with powerups
        powerups = new GameObject[] { powerupSlow, powerupInfAmmo, powerupHeal};
        // Initialize quantity of enemies to be spawned for the first wave
        enemiesToSpawn = 2;
        // Spawn the first wave
        SpawnEnemyWave();
    }

    void Update()
    {
        // While the game is running add current enemies of each type to their respective lists
        if (GameManager.isGameActive){
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            slowEnemies = GameObject.FindGameObjectsWithTag("SlowEnemy");
            fastEnemies = GameObject.FindGameObjectsWithTag("FastEnemy");
            // Check if all lists are empty to know round has been cleared annd to start a new one
            if (enemies.Length <= 0 && slowEnemies.Length <= 0 && fastEnemies.Length <= 0)
            {
                // Start the new round and update round on ui
                waveNumber++;
                GameManager.Instance.UpdateWave();
                SpawnEnemyWave();

                // For every wave after round 2, increase the speed of enemies
                if (waveNumber > 2)
                {
                    enemySpeed += 2000f;
                    Enemy.SetSpeed(enemySpeed);
                }
            }
        }
    }

    private Vector3 GenerateSpawnPosition(int spawnHeight)
    {
        // Create a vector3 to store position of the object being spawned
        Vector3 randomPos;

        // Retrieve current player position
        playerPosX = playerRb.position.x;
        playerPosZ = playerRb.position.z;

        do
        {
            // Randomly choose x/y position for the object
            spawnPosX = UnityEngine.Random.Range(-spawnRange + platformOffsetX, spawnRange + platformOffsetX);
            spawnPosZ = UnityEngine.Random.Range(-spawnRange + platformOffsetZ, spawnRange + platformOffsetZ);
        }   // Repeat until random position isn't too close vertically or isn't too close horizontally
            // Can be too close in one direction as they will be outside of a dangerously close area around the player
            // Implemented to stop unlucky roudn spawns on top of the player
        while ( !( (spawnPosX > playerPosX + spawnCushionRadius) || (spawnPosX < playerPosX - spawnCushionRadius) )||!( (spawnPosZ > playerPosZ + spawnCushionRadius) || (spawnPosZ < playerPosZ - spawnCushionRadius) ) );
        
        // Add positions to the vector and set custom height: enemy at 0, powerups slightly floating
        randomPos = new Vector3(spawnPosX, spawnHeight, spawnPosZ);

        return randomPos;
    }

    private void SpawnEnemyWave()
    {
        // Call any changes that need to be made to the wave
        waveChanges();
        Enemy.SetSpeed(enemySpeed);
        // Create normal enemies and spawn in random locations
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, GenerateSpawnPosition(0), enemyPrefab.transform.rotation);
        }
        // Create fast enemies and spawn in random locations 
        for (int i = 0; i < fastEnemiesToSpawn; i++)
        {
            GameObject fastEnemy = Instantiate(fastEnemyPrefab, GenerateSpawnPosition(0), fastEnemyPrefab.transform.rotation);
        }
        // Create slow enemies and spawn in random locations
        for (int i = 0; i < slowEnemiesToSpawn; i++)
        {
            GameObject slowEnemy = Instantiate(slowEnemyPrefab, GenerateSpawnPosition(0), slowEnemyPrefab.transform.rotation);
        }
    }

     private void waveChanges()
    {
        // Every wave increase normal enemies by 1
        enemiesToSpawn += 1;
        // Every thrid wave increase fast ennemies by 1 and create a radom powerup
        if (waveNumber % 3 == 0)
        {
            fastEnemiesToSpawn += 1;
            int powerupChoice = UnityEngine.Random.Range(0, 3);
            Instantiate(powerups[powerupChoice], GenerateSpawnPosition(4), powerups[powerupChoice].transform.rotation);
        }
        // Every fifth wave icrease slow enemies by 2
        if (waveNumber % 5 == 0)
        {
            slowEnemiesToSpawn += 2;   
        }
    }
}
