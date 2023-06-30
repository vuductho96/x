using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour
{
    public Animator anim;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Slider healthSlider;
    private float damageAmount;

    private void Start()
    {
     
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        anim = GetComponent<Animator>();
    }

    public void UpdateHealth(float newHealth)
    {
        currentHealth = newHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
    }

    public void TakeDamageFromPlayer(float damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealth(currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        Debug.Log("Enemy has died!");

        // Destroy the enemy object
        Destroy(gameObject);
    }

    public void ApplyKnockBack(Vector3 knockBackDirection)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(knockBackDirection, ForceMode.Impulse);
        }
    }
}
