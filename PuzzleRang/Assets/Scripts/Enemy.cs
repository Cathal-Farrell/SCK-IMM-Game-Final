using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static float speed;      // Enemy speed
    private Rigidbody enemyRb;      // Enemies rigid body
    private GameObject player;      // Player object
    public AudioClip deathSound;    // Custom death sound

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();    // Retrieve enemies rigidbody
        player = GameObject.Find("Player");     // Retrieve player object
    }

    // Update is called once per frame
    void Update()
    {
        // While game is running, make enemy look towards player and move forward
        // Speed varies based on enemy type
        if (GameManager.isGameActive){
            transform.LookAt(player.transform.position);
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            if (CompareTag("Enemy"))
            {
                enemyRb.AddForce(lookDirection * speed * Time.deltaTime);           // Normal enemies move at normal speeed
            }
            if (CompareTag("FastEnemy"))
            {
                enemyRb.AddForce(lookDirection * speed * 3f * Time.deltaTime);      // Small enemies move faster
            }
            if (CompareTag("SlowEnemy"))
            {
                enemyRb.AddForce(lookDirection * speed * 0.75f * Time.deltaTime);   // Big enemies move slower
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Enemy dies when bullet collides
        if (collision.gameObject.CompareTag("Bullet")) {
            Destroy(collision.gameObject); // Destroy the bullet
            Destroy(gameObject);    // Destroy the enemy

            GameManager.Instance.PlaySFXByIndex(2); // Play enemy death sound
        }
    }

    // Changing speed affects speed of all instances of enemies
    public static void SetSpeed(float speedChange)
    {
        speed = speedChange;
    }
}
