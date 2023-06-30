using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthBar1 : MonoBehaviour
{
    public Animator anim;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Slider healthSlider; // Reference to the slider component for the health bar
    public GameObject Youlose;
    public AudioClip Diesound;
    public AudioClip HitSound;
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Initialize the health bar slider
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        anim = GetComponent<Animator>();
        Youlose.gameObject.SetActive(false);
    }

    public void UpdateHealth(float newHealth)
    {
        // Update the current health value
        currentHealth = newHealth;

        // Clamp the current health value between 0 and max health
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Update the health bar value
        healthSlider.value = currentHealth;
    }
    public void TakeDamageFromEnemy(float damageAmount)
    {
        anim.SetTrigger("Hit");
        audioSource.PlayOneShot(HitSound);
        // Subtract the damage amount from the current health
        currentHealth -= damageAmount;

        // Update the health bar with the new health value
        UpdateHealth(currentHealth);

        // Check if the player's health reaches zero or below
        if (currentHealth <= 0f)
        {
            // Perform player death logic here
            Die();
        }
    }
    public void Tr1yAgain()
    {
        SceneManager.LoadScene("MainGame");
    }
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2.5f);

        // Destroy the enemy object
        Destroy(gameObject);
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        Debug.Log("Enemy has died!");
        audioSource.PlayOneShot(Diesound);
        // Activate the Youlose text
        Youlose.gameObject.SetActive(true);

        // Start the coroutine to delay the destruction
        StartCoroutine(DelayedDestroy());

        // Handle the player's death logic here
        // You can perform additional actions like showing a game over screen or restarting the level.
    }

}
