using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip ZombieSound;
    public float enemyAttck = 20f;
   public Transform target; // Reference to the player's transform
    public float movementSpeed = 1.5f;
    public float attackRange = 2f;
    public float detectionRange = 10f; // Range at which the enemy detects the player
    public float attackCooldown = 2f; // Cooldown time between attacks
    private Animator animation;
    private bool canAttack = true;
    private bool isFound; // Flag to track attack cooldown
    private PlayerHealthBar1 playerHealthBar;

    private enum EnemyState
    {
        Patrol,
        Chase
    }
  
    private EnemyState currentState;
    private void Start()
    {
     
        // Find and assign the player's transform as the target
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animation = GetComponent<Animator>();
        playerHealthBar = target.GetComponent<PlayerHealthBar1>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(ZombieSound);
        // Start in the patrol state
        currentState = EnemyState.Patrol;
    } 
    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                ChasePlayer();
                break;
        }
    }

    private void Patrol()
    {
        // Implement your patrol logic here
        // Example: Move between predefined waypoints

        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, target.position) <= detectionRange)
        {
            // Switch to the chase state if the player is detected
            currentState = EnemyState.Chase;
        }
    }

    private void ChasePlayer()
    {
        animation.SetFloat("Enemy", 1f, 0.1f, Time.deltaTime);
      
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Keep the enemy at the same height
        direction.Normalize();
        transform.position += direction * movementSpeed * Time.deltaTime;
        
        // Move towards the player
       

        // Rotate the enemy to face the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            // Attack the player
            Attack();
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            animation.SetTrigger("ENEMYATTACK");
            playerHealthBar.TakeDamageFromEnemy(enemyAttck); // Change the damage amount as needed

            // Start the attack cooldown
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;

        // Wait for the specified cooldown time
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

}
