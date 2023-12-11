using System;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public event Action<PlayerStats> OnPlayerStatsInitialized;

    [SerializeField]
    public float maxHealth = 50f;
    [SerializeField]
    private GameObject deathChunkParticle, hitParticle, deathBloodParticle;
    public GameObject pauseMenu;
    public float currentHealth;
    private Animator anim;
    private GameManager GM;
    private bool die = false;
    public int damageAmount = 10;
    private bool isTakingDamage = false;
    private bool damage = false;
    public float damageInterval = 1.0f;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI healthText2;
    public event Action<float, float> OnHealthChanged;

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

        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
    healthText.text = $"HP = {GetCurrentHealth()}/{GetMaxHealth()}";
    healthText2.text = $"HP: {GetCurrentHealth()} / {GetMaxHealth()}";
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
            anim.SetBool("damage", damage);
            float normalizedHealth = GetCurrentHealthNormalized(currentHealth, maxHealth);

            FindObjectOfType<HealthBar>().UpdateHealthBar(PlayerStats.GetCurrentHealthNormalized(currentHealth, maxHealth));
            UpdateHealthText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            DecreaseHealth(damageAmount);
            isTakingDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            isTakingDamage = false;
        }
    }

    private void Die()
    {
        die = true;
        anim.SetBool("Die", die);

        // Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        // Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        GM.gameOver();
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth += amount;

        // Ensure health doesn't exceed the maximum
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthText();

        // Trigger the OnHealthChanged event
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

     public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;

        // Pastikan current health tidak melebihi maximum baru
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthText();

        // Memanggil fungsi UpdateHealthBar dari objek HealthBar
        FindObjectOfType<HealthBar>().UpdateHealthBar(GetCurrentHealthNormalized(currentHealth, maxHealth));
    }
}