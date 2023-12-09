using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionUIDisplay : MonoBehaviour
{
    public Toggle hpPotionToggle;
    public Toggle manaPotionToggle;
    public TextMeshProUGUI potionInfoText;
    public TextMeshProUGUI potionDescriptionText; // Tambahkan TextMeshProUGUI untuk menampilkan deskripsi potion
    public PotionManager potionManager;

    void Start()
    {
        hpPotionToggle.isOn = true;
        hpPotionToggle.onValueChanged.AddListener(OnToggleChanged);
        manaPotionToggle.onValueChanged.AddListener(OnToggleChanged);
        UpdatePotionInfoText();
    }

    void OnToggleChanged(bool isOn)
    {
        UpdatePotionInfoText();
        UpdatePotionDescriptionText(); // Perbarui deskripsi setiap kali toggle berubah
    }

    void UpdatePotionInfoText()
    {
        string potionType = hpPotionToggle.isOn ? "HpPotion" : "ManaPotion";
        int currentCount = potionType == "HpPotion" ? potionManager.currentHpPotionCount : potionManager.currentManaPotionCount;
        int maxCount = potionType == "HpPotion" ? potionManager.maxHpPotionCount : potionManager.maxManaPotionCount;
        string infoText = $"{potionType} ({currentCount}/{maxCount})";
        potionInfoText.text = infoText;
    }

    void UpdatePotionDescriptionText()
    {
        string potionType = hpPotionToggle.isOn ? "HpPotion" : "ManaPotion";
        string description = GetPotionDescription(potionType);
        potionDescriptionText.text = description;
    }

    string GetPotionDescription(string potionType)
    {
        // Tambahkan deskripsi potion sesuai kebutuhan
        if (potionType == "HpPotion")
        {
            return "A potion that restores health.";
        }
        else if (potionType == "ManaPotion")
        {
            return "A potion that restores mana.";
        }

        return "Invalid Potion Type";
    }
}
