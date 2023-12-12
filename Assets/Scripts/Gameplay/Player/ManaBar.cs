using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
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
            playerStats.OnManaChanged += HandleManaChanged;
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
            playerStats.OnManaChanged += HandleManaChanged;
        }
    }

    void UnsubscribeFromEvents()
    {
        if (playerStats != null)
        {
            playerStats.OnPlayerStatsInitialized -= HandlePlayerStatsInitialized;
            playerStats.OnManaChanged -= HandleManaChanged;
        }
    }

    void HandlePlayerStatsInitialized(PlayerStats initializedPlayerStats)
    {
        // Update the mana bar when the player stats are initialized
        float normalizedMana = GetCurrentManaNormalized(initializedPlayerStats.GetCurrentMana(), initializedPlayerStats.GetMaxMana());
        UpdateManaBar(normalizedMana);
    }

    void HandleManaChanged(float currentMana, float maxMana)
    {
        // Update the mana bar when the player's mana changes
        float normalizedMana = GetCurrentManaNormalized(currentMana, maxMana);
        UpdateManaBar(normalizedMana);
    }

    public void UpdateManaBar(float normalizedMana)
    {
        SetSize(normalizedMana);

        if (normalizedMana < 0.1f)
        {
            barImage.color = Color.blue;
        }
        else
        {
            barImage.color = Color.blue; // Reset color to white if mana is above the threshold
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

    private float GetCurrentManaNormalized(float currentMana, float maxMana)
    {
        // Check for division by zero or invalid values
        if (maxMana > 0f && !float.IsNaN(currentMana) && !float.IsInfinity(currentMana))
        {
            return currentMana / maxMana;
        }
        else
        {
            // Log an error and return a default value
            Debug.LogError("Invalid mana values: currentMana=" + currentMana + ", maxMana=" + maxMana);
            return 0f;
        }
    }
}
