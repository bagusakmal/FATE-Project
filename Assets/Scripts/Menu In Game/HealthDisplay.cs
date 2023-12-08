using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    void Update()
    {
        float totalHealth = Health.totalHealth; 
        int healthPercentage = Mathf.RoundToInt(totalHealth * 100f);

        healthText.text = "HP : " + healthPercentage.ToString() + " / 100 ";
    }
}
