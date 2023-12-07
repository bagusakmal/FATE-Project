using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform bar;
    private Image barImage;

    void Awake()
    {
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
    }

    void Start()
    {
        // Find the PlayerStats component in the scene
        PlayerStats playerStats = GameObject.FindWithTag("Player")?.GetComponent<PlayerStats>();

        // Subscribe to the PlayerStats initialization event
        if (playerStats != null)
        {
            playerStats.OnPlayerStatsInitialized += HandlePlayerStatsInitialized;
        }
        else
        {
            Debug.LogError("PlayerStats component not found!");
        }
    }

    void HandlePlayerStatsInitialized(PlayerStats playerStats)
    {
        // Update the health bar when the player stats are initialized
        UpdateHealthBar(PlayerStats.GetCurrentHealthNormalized(playerStats.GetCurrentHealth(), playerStats.GetMaxHealth()));
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
}
