using TMPro;
using UnityEngine;

public class AttackDisplay : MonoBehaviour
{
    public PlayerCombatController playerCombatController;
    public TextMeshProUGUI damageText;

    void Update()
    {
        // Update the displayed damage value
        UpdateDamageText();
    }

    void UpdateDamageText()
    {
        // Check if the playerCombatController is assigned
        if (playerCombatController != null)
        {
            // Get the current attack1Damage value
            float currentDamage = playerCombatController.GetAttack1Damage();

            // Update the TextMeshPro component with the current damage value
            if (damageText != null)
            {
                damageText.text = "ATTACK = " + currentDamage.ToString();
            }
        }
    }
}
