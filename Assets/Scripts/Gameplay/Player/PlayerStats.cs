using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Declare the event within the PlayerStats class
    public event Action<PlayerStats> OnPlayerStatsInitialized;

    [SerializeField]
    public static float maxHealth = 50f;
    

    [SerializeField]
    private GameObject deathChunkParticle, hitParticle, deathBloodParticle;
    public GameObject pauseMenu;

    private float currentHealth;

    private Animator anim;

    private GameManager GM;
    private bool die = false;
    // private bool isPaused = false;
    public int damageAmount = 10;
    private bool isTakingDamage = false; // Flag untuk menandakan apakah pemain sedang mengalami damage
    private bool damage = false;
    public float damageInterval = 1.0f; // Interval waktu antara setiap serangan
    // Add these methods to get current health and max health
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
         // Trigger the initialization event
        OnPlayerStatsInitialized?.Invoke(this);
    }

     private void Update()
    {
        // // Jika pemain sedang mengalami damage dan game tidak sedang di-pause
        // if (isTakingDamage && !isPaused)
        // {
        //     StartCoroutine(TakeDamageRepeatedly());
        // }
    }

   public static float GetCurrentHealthNormalized(float currentHealth, float maxHealth)
{
    // Check for division by zero or invalid values
    if (maxHealth > 0f && !float.IsNaN(currentHealth) && !float.IsInfinity(currentHealth))
    {
        return currentHealth / maxHealth;
    }
    else
    {
        // Log an error and return a default value
        Debug.LogError("Invalid health values: currentHealth=" + currentHealth + ", maxHealth=" + maxHealth);
        return 0f;
    }
}
    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0.0f && !die)
        {
            Die();
        }
        else
        {
            Instantiate(hitParticle, anim.transform.position, Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f)));
            damage = true;
            anim.SetBool("damage",damage);
            // Pass the correct parameters to GetCurrentHealthNormalized
            float normalizedHealth = GetCurrentHealthNormalized(currentHealth, maxHealth);

             // Update the health bar when the player takes damage
            FindObjectOfType<HealthBar>().UpdateHealthBar(PlayerStats.GetCurrentHealthNormalized(currentHealth, maxHealth));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike")) // Ganti dengan tag yang sesuai pada tile spike
        {
            DecreaseHealth(damageAmount);
            isTakingDamage = true; // Pemain mengalami damage

        }
    }
     private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Spike")) // Ganti dengan tag yang sesuai pada tile spike
        {
            isTakingDamage = false; // Pemain meninggalkan area spike
        }
    }

    // private IEnumerator TakeDamageRepeatedly()
    // {
    //     while (isTakingDamage)
    //     {
    //         yield return new WaitForSeconds(damageInterval);

    //         DecreaseHealth(damageAmount);
    //     }
    // }

    private void Die()
    {
        die = true;
        anim.SetBool("Die", die);

        // Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        // Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.gameOver();
    }
}
