using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    // Reference to the UI Text object
    public Text healthText; 
    
    // a coroutine is a function that can suspend its execution (yield) 
    // until the specified conditions are met and then resume from where it left off. 
    private Coroutine healthIncreaseCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText(); 
        // Start coroutine for periodic health increase
        healthIncreaseCoroutine = StartCoroutine(PeriodicHealthIncrease());
    }

    void TakeEnemyBulletDamage()
    {
        // Reduce Player health
        currentHealth -= 10;

        // Check the condition
        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthText(); 
    }

    void Die()
    {
        Debug.Log("Player Died");
        // enable the cursor to click
        Cursor.lockState = CursorLockMode.None;
        // Load the "GameOver" scene
        SceneManager.LoadScene("GameOver"); 

        // Additional death logic
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is a bullet fired by the enemy
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            TakeEnemyBulletDamage();

            Destroy(collision.gameObject);

            Debug.Log("Player health reduced by 20.");
        }
    }

    void UpdateHealthText()
    {
        // Update the UI text with the current health value
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString();
        }
        healthBar.value = currentHealth;

        // Check if health is less than 30 to start the periodic increase
        if (currentHealth <= 30 && healthIncreaseCoroutine == null)
        {
            healthIncreaseCoroutine = StartCoroutine(PeriodicHealthIncrease());
        }
    }

    IEnumerator PeriodicHealthIncrease()
    {
        while (currentHealth < 50)
        {
            yield return new WaitForSeconds(2f); // Wait for 3 seconds
           
            currentHealth += 5;
            
            UpdateHealthText(); 
        }
        healthIncreaseCoroutine = null; // Reset coroutine when health reaches 50
    }
}
