using System.Collections;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    // Enemy Movement/Health properties
    public float movementSpeed = 4f;
    public int maxHealth = 100;
    public int currentHealth;

    // Enemy Attacking/Shooting Properties
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 20f;
    public float fireRate = 2f;
    private float nextFireTime;
    public float attackRange = 15f;
    public GameObject deathEffect;

    // Declaring Enumerator List containing 2 constants 
    public enum weaponModel
    {
        Pistol,
        Rifle
    }

    // Declaring Enumerator/Struct List containing 2 constants 
    public weaponModel currentWeaponModel;

    // For Navigating Player 
    public Transform player;

    // Distance between enemy and player at certain point
    public float desiredDistance = 4f;

    // Other properties
    private Animator animator;
    private bool isDead = false;
    private bool isFollowingPlayer = false; // Flag to indicate if enemy is following the player

    // For Dropping/Instantiating Ammo
    public GameObject rifleAmmoPrefab;
    public GameObject bloodEffectPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        // animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            // Check if the player is within attack range
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                // Set flag to start following the player
                isFollowingPlayer = true;
            }

            if (isFollowingPlayer)
            {
                // Calculate the distance between enemy and player
                float distance = Vector3.Distance(transform.position, player.position);

                // Check if the distance is less than the desired distance
                if (distance < desiredDistance)
                {
                    // Move the enemy towards the player
                    Vector3 directionToPlayer = (transform.position - player.position).normalized;
                    // Freeze Y position
                    directionToPlayer.y = 0f;
                    // moves the Enemy towards or away from the player based on directionToPlayer
                    transform.position += directionToPlayer * movementSpeed * Time.deltaTime;
                }
                else
                {
                    // Move the enemy towards the player
                    Vector3 directionToPlayer = (player.position - transform.position).normalized;
                    // Freeze Y position
                    directionToPlayer.y = 0f;
                    // moves the Enemy towards or away from the player based on directionToPlayer
                    transform.position += directionToPlayer * movementSpeed * Time.deltaTime;
                }

                // Rotate towards the player
                transform.LookAt(player);

                // Attack the player
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
    }

    private void Shoot()
    {
        // Check if it's time to fire and the enemy is within range to attack
        if (Time.time >= nextFireTime && isFollowingPlayer)
        {
            // animator.SetTrigger("FireRifle");

            // Instantiate bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            // Set bullet velocity towards the player
            Vector3 directionToPlayer = (player.position - bulletSpawn.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = directionToPlayer * bulletSpeed;

            // Destroy the bullet after 3 seconds
            StartCoroutine(DestroybulletPrefabLife(bullet, 3f));

            // Play shooting sound
            SoundManager.Instance.PlayShootingSound((Weapon.weaponModel)currentWeaponModel);

            // Update the next fire time
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    // Coroutine to destroy bullet prefab after a delay
    private IEnumerator DestroybulletPrefabLife(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    // For Enemy Health
    void TakePlayerBulletDamage(Vector3 hitPoint)
    {
        // Reduce Enemy Health
        currentHealth -= 20;

        // Instantiate blood effect at hit point
        GameObject bloodEffect = Instantiate(bloodEffectPrefab, hitPoint, Quaternion.identity);
        
        // Destroy the blood effect after a delay
        StartCoroutine(DestroyBloodEffect(bloodEffect, 0.6f)); // Adjust delay as needed

        // Check health Condition
        if (currentHealth <= 0)
        {
            Die();
        }
        // Set flag to start following the player
        isFollowingPlayer = true;
    }

    private IEnumerator DestroyBloodEffect(GameObject bloodEffect, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bloodEffect);
    }

    void Die()
    {
        // If the enemy is already dead, do nothing
        if (isDead)
            return;

        Debug.Log("Enemy Died");
        // If Dead then
        isDead = true;

        // Play death animation
        // animator.SetTrigger("Dead");

        // Drop Rifle ammo prefab
        Instantiate(rifleAmmoPrefab, transform.position, Quaternion.identity);

        // Disable enemy AI
        this.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is a bullet fired by the player
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            TakePlayerBulletDamage(collision.contacts[0].point); // Pass the hit point to the damage method

            Destroy(collision.gameObject);

            Debug.Log("Enemy health reduced by 20.");
        }
    }
}
