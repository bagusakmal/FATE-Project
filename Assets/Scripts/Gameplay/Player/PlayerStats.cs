using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public event Action<PlayerStats> OnPlayerStatsInitialized;

    [SerializeField]
    public float maxHealth = 50f, maxMana = 50f;
    [SerializeField]
    private GameObject deathChunkParticle, hitParticle, deathBloodParticle;
    [SerializeField]
    private float manaRegenerationRate = 0.001f;
    public GameObject pauseMenu;
    public float currentHealth, currentMana;
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
    [SerializeField]
    private TextMeshProUGUI manaText;
    [SerializeField]
    private TextMeshProUGUI manaText2;
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnManaChanged;
    private Coroutine manaRegenerationCoroutine;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = 30f;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();
        // Trigger the initialization event
        OnPlayerStatsInitialized?.Invoke(this);

        UpdateHealthText();
        UpdateManaText();
    }

    private void UpdateHealthText()
    {
    healthText.text = $"HP = {GetCurrentHealth()}/{GetMaxHealth()}";
    healthText2.text = $"HP: {GetCurrentHealth()} / {GetMaxHealth()}";
    }
    private void UpdateManaText()
    {
        manaText.text = $"MP = {GetCurrentMana()}/{GetMaxMana()}";
        manaText2.text = $"MP: {GetCurrentMana()} / {GetMaxMana()}";
    }

    private void Update()
    {
        // // Jika pemain sedang mengalami damage dan game tidak sedang di-pause
        // if (isTakingDamage && !isPaused)
        // {
        //     StartCoroutine(TakeDamageRepeatedly());
        // }

        // Uncomment the following lines if you want to handle mana regeneration
       if (!isTakingDamage && manaRegenerationCoroutine == null)
    {
        manaRegenerationCoroutine = StartCoroutine(RegenerateMana());
    }
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

    public void DecreaseMana(float amount)
    {
        currentMana -= amount;

        // Ensure mana doesn't go below zero
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

        UpdateManaText();

        // Trigger the OnManaChanged event
        OnManaChanged?.Invoke(currentMana, maxMana);
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

    public void IncreaseMana(float amount)
    {
        currentMana += amount;

        // Ensure mana doesn't exceed the maximum
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

        UpdateManaText();

        // Trigger the OnManaChanged event
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    private IEnumerator RegenerateMana()
    {
    float elapsedTime = 0f;
    float regenerationInterval = 1f; // 1 minute regeneration interval

    while (currentMana < maxMana)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= regenerationInterval)
        {
            IncreaseMana(1);
            elapsedTime = 0f; // Reset the timer
        }

        yield return null;
    }

    // Set the coroutine to null to allow it to be started again in the Update function
    manaRegenerationCoroutine = null;
    }
}