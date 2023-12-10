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
        FindPlayerStats();

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

    void FindPlayerStats()
    {
        // Find the PlayerStats component in the scene
        playerStats = GameObject.FindWithTag("Player")?.GetComponent<PlayerStats>();
        
        // Check if the playerStats is still null
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found!");
        }
    }

    void OnEnable()
    {
        // Subscribe to events when the script is enabled
        FindPlayerStats();
        SubscribeToEvents();
    }

    void OnDisable()
    {
        // Unsubscribe from events when the script is disabled
        UnsubscribeFromEvents();
    }

    void SubscribeToEvents()
    {
        if (playerStats != null)
        {
            playerStats.OnPlayerStatsInitialized += HandlePlayerStatsInitialized;
            playerStats.OnHealthChanged += HandleHealthChanged;
        }
    }

    void UnsubscribeFromEvents()
    {
        if (playerStats != null)
        {
            playerStats.OnPlayerStatsInitialized -= HandlePlayerStatsInitialized;
            playerStats.OnHealthChanged -= HandleHealthChanged;
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
            barImage.color = Color.green;
        }
        else
        {
            barImage.color = Color.green; // Reset color to white if health is above the threshold
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
