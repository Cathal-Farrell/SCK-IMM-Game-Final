using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;                 // Bullet speed
    public float bulletLifetime = 2f;   // Lifetime for each bullet in seconds
    private int maximumAmmo = 5;        // Maximum ammo capacity
    private int ammo;                   // Current ammo available
    private bool isReloading = false;   // Is currently reloading
    public bool infiniteAmmo = false;   // Currently has infinite ammo powerup active    

    public GameObject player;           // Player's game object
    public GameObject bulletPrefab;     // Bullet prefab

    private Animator playerAnimator;    // Reference to Animator

    void Start()
    {
        ammo = maximumAmmo;                                 // Fill current ammo
        playerAnimator = player.GetComponent<Animator>();   // Get the Animator component
    }

    void Update()
    {
        // While game is active, check for space button press, R button press, or for current ammo depletion
        if (GameManager.isGameActive){
            // If space bar pressed: spawn a bullet, and reduce current ammo by one as long as infinite ammo poweup is nnot active
            if (Input.GetKeyDown(KeyCode.Space) && ammo > 0 && !isReloading)
            {
                SpawnBullet();
                if (!infiniteAmmo)
                {
                    ammo--;
                    GameManager.Instance.UpdateAmmo(1);
                }

                // Trigger the shoot animation
                playerAnimator.SetTrigger("Shoot");

                // Play sound effect
                GameManager.Instance.PlaySFXByIndex(0);
            }
            // If R button pressed or curret ammo reaches 0, start the reload coroutine and reload the weapon
            else if ((ammo <= 0 && !isReloading) || (Input.GetKeyDown(KeyCode.R) && !isReloading))
            {
                StartCoroutine(ReloadAmmo());

                // Trigger the reload animation
                playerAnimator.SetTrigger("reloadingAnim");

                // Play reload sound effect
                GameManager.Instance.PlaySFXByIndex(1);
            }
        }

    }

    void SpawnBullet()
    {
        // Instantiate the bullet at the player's position and rotation, but not as a child of the player
        GameObject bullet = Instantiate(bulletPrefab, player.transform.position, player.transform.rotation);

        // Set the bullet's speed, controlled by the BulletMovement script
        BulletMovement bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletMovement.speed = speed;

        // Destroy the bullet after its lifetime ends
        Destroy(bullet, bulletLifetime);
    }

    IEnumerator ReloadAmmo()
    {
        isReloading = true;                     // Set isReloading to be true so reloading is not spammed
        yield return new WaitForSeconds(3f);    // Wait 3 seconds
        ammo = maximumAmmo;                     // Max out ammo count
        GameManager.Instance.UpdateAmmo(5);     // Update the ui
        isReloading = false;                    // Set isReloading to false again
    }

    public void SetMaxAmmo()
    {
        ammo = maximumAmmo;                     // Max out ammo
        GameManager.Instance.UpdateAmmo(5);     // Update the ui
    }

    public void SetInfiniteAmmo(bool isInfiniteAmmo)
    {
        infiniteAmmo = isInfiniteAmmo;
    }
}
