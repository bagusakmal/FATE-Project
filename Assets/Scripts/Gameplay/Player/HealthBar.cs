using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform bar;
    private Image barImage;

    private PlayerStats playerStats;

    void Awake()
    {
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
    }

    void Start()
    {
        // Find the PlayerStats component in the scene
        playerStats = GameObject.FindWithTag("Player")?.GetComponent<PlayerStats>();

        // Subscribe to the PlayerStats initialization event
        if (playerStats != null)
        {
            playerStats.OnPlayerStatsInitialized += HandlePlayerStatsInitialized;
            playerStats.OnHealthChanged += HandleHealthChanged;
        }
        else
        {
            Debug.LogError("PlayerStats component not found!");
        }
    }

    void HandlePlayerStatsInitialized(PlayerStats initializedPlayerStats)
    {
        // Update the health bar when the player stats are initialized
        float normalizedHealth = GetCurrentHealthNormalized(initializedPlayerStats.GetCurrentHealth(), initializedPlayerStats.GetMaxHealth());
        UpdateHealthBar(normalizedHealth);
    }

    void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        // Update the health bar when the player's health changes
        float normalizedHealth = GetCurrentHealthNormalized(currentHealth, maxHealth);
        UpdateHealthBar(normalizedHealth);
    }

    public void UpdateHealthBar(float normalizedHealth)
    {
        SetSize(normalizedHealth);

        if (normalizedHealth < 0.1f)
        {
            barImage.color = Color.red;
        }
    }

    private void SetSize(float size)
    {
        if (!float.IsNaN(size) && !float.IsInfinity(size))
        {
            bar.localScale = new Vector3(size, 1f, 1f);
        }
        else
        {
            Debug.LogError("Invalid size value: " + size);
        }
    }

    private float GetCurrentHealthNormalized(float currentHealth, float maxHealth)
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
}
