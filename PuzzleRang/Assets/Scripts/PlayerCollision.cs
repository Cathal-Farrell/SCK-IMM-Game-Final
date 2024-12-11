using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class PlayerCollision : MonoBehaviour
{

    private int healthPoints = 10;      // Players health points
    public GameObject bulletManager;    // Bullet manager cotrols bullet spawning
    private BulletController playerBC;  // Bullet controller attached to bullet manager
    public AudioClip maxAmmo;           // Custom audio for max ammo pickup and 

    // Start is called before the first frame update
    void Start()
    {
        playerBC = bulletManager.GetComponent<BulletController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // If collision is with bullet, ignore 
        if (other.gameObject.CompareTag("Bullet"))
        {
            return;
        }
        // If collision is with an enemy
        // Reduce hp based on damage certain ennemies deal
        // Update healthpoints in ui
        else if (other.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.PlaySFXByIndex(6);
            healthPoints -= 2;
            GameManager.Instance.UpdateHealth(-2);
        }
        else if (other.gameObject.CompareTag("FastEnemy"))
        {
            GameManager.Instance.PlaySFXByIndex(6);
            healthPoints -= 1;
            GameManager.Instance.UpdateHealth(-1);
        }
        else if (other.gameObject.CompareTag("SlowEnemy"))
        {
            GameManager.Instance.PlaySFXByIndex(6);
            healthPoints -= 3;
            GameManager.Instance.UpdateHealth(-3);
        }
        // If collision is with a powerup
        // Give respective buff
        else if (other.gameObject.CompareTag("InfAmmo"))
        {
            GameManager.Instance.PlaySFXByIndex(3);     // Play custom audio file
            playerBC.SetMaxAmmo();                      // Max out current ammo
            playerBC.SetInfiniteAmmo(true);             // Give infinite ammo
            GameManager.Instance.UpdateAmmo(0);
            StartCoroutine(InfiniteAmmoCountdown());    // Start coroutine for 10 secs
        }
        else if (other.gameObject.CompareTag("Slow"))
        {
            GameManager.Instance.PlaySFXByIndex(4);     // Play custom audio file
            Enemy.SetSpeed(SpawnManager.enemySpeed/2);  // Cut ennemy speed in half
            StartCoroutine(EnemySlowCountdown());       // Start coroutine for 10 secs
        }
        else if (other.gameObject.CompareTag("Heal"))
        {
            GameManager.Instance.PlaySFXByIndex(5);     // Play custom audio file
            healthPoints += 5;                          // Give 5 health points
            GameManager.Instance.UpdateHealth(5);       // Update health on ui
        }
        // Always destroy the collided object
        Destroy(other.gameObject);
        // Check if player dies after every collision
        if (healthPoints <= 0)
        {
            GameIsOver();
        }
    }
      

void GameIsOver()
{
    // Stop time and update game state
    Time.timeScale = 0f;
    GameManager.isGameActive = false;

    // Show the Game Over screen
    GameManager.Instance.GameOver();

    // Stop background music
    GameManager.Instance.StopBackgroundMusic();

    // Play the "Game Over" music at index 1 (adjust index as needed)
    GameManager.Instance.PlayMusicByIndex(0);
}



    IEnumerator InfiniteAmmoCountdown()
    {
        yield return new WaitForSeconds(10);    // 10 second coroutine
        playerBC.SetInfiniteAmmo(false);        // Turn off inf ammo after
        
    }

    IEnumerator EnemySlowCountdown()
    {   
        yield return new WaitForSeconds(10);        // 10 second coroutine
        Enemy.SetSpeed(SpawnManager.enemySpeed);    // Set enemy speed to normal after
        
    }
}
