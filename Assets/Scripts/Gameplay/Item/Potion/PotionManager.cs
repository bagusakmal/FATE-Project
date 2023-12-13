using System.Collections;
using TMPro;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    public int maxHpPotionCount = 5;
    public int maxManaPotionCount = 5;
    public TextMeshProUGUI hpPotionCountText;
    public TextMeshProUGUI manaPotionCountText;
    public int healingAmount = 20; // Adjust the healing amount as needed

    public int currentHpPotionCount = 0;
    public int currentManaPotionCount = 0;

    // Reference to the PlayerStats script
    private PlayerStats playerStats;

    void Start()
    {
        // Muat jumlah item potion dari PlayerPrefs saat memulai
        LoadPotionCount();
        UpdatePotionCountText();

        // Find and cache the PlayerStats component
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Input untuk memanggil fungsi hpPotion
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseHpPotion();
        }

        // Input untuk memanggil fungsi manaPotion
        if (Input.GetKeyDown(KeyCode.J))
        {
            UseManaPotion();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cek apakah masih bisa mengambil potion
            if ((currentHpPotionCount < maxHpPotionCount) || (currentManaPotionCount < maxManaPotionCount))
            {
                currentHpPotionCount++;

                // Use HP Potion and check if the inventory is not full
                if (CollectHpPotion())
                {
                    // Destroy the potion object
                    Destroy(gameObject);

                    // Simpan jumlah item potion ke PlayerPrefs
                    SavePotionCount();

                    // Update tampilan jumlah potion
                    UpdatePotionCountText();
                }
            }
            else
            {
                Debug.Log("Inventory penuh! Tidak dapat mengambil lebih banyak potion.");
            }
        }
    }

    void UpdatePotionCountText()
    {
        hpPotionCountText.text = currentHpPotionCount + "/" + maxHpPotionCount;
        manaPotionCountText.text = currentManaPotionCount + "/" + maxManaPotionCount;
    }

    void SavePotionCount()
    {
        // Simpan jumlah item potion ke PlayerPrefs
        PlayerPrefs.SetInt("HpPotionCount", currentHpPotionCount);
        PlayerPrefs.SetInt("ManaPotionCount", currentManaPotionCount);
        PlayerPrefs.Save();
    }

    void LoadPotionCount()
    {
        // Muat jumlah item potion dari PlayerPrefs
        if (PlayerPrefs.HasKey("HpPotionCount"))
        {
            currentHpPotionCount = PlayerPrefs.GetInt("HpPotionCount");
        }

        if (PlayerPrefs.HasKey("ManaPotionCount"))
        {
            currentManaPotionCount = PlayerPrefs.GetInt("ManaPotionCount");
        }
    }

    void UseHpPotion()
    {
    // Cek apakah masih ada hpPotion yang tersedia
    if (currentHpPotionCount > 0)
    {
        // Gunakan hpPotion dan tambahkan logika peningkatan HP di sini
        if (playerStats != null)
        {
            playerStats.IncreaseHealth(healingAmount);
        }

        currentHpPotionCount--;

        // Simpan jumlah item potion ke PlayerPrefs
        SavePotionCount();

        // Update tampilan jumlah potion
        UpdatePotionCountText();
    }
    else
    {
        Debug.Log("Tidak ada HP Potion yang tersedia.");
    }
    }

    void UseManaPotion()
    {
        // Cek apakah masih ada manaPotion yang tersedia
        if (currentManaPotionCount > 0)
        {
            // Gunakan manaPotion dan tambahkan logika peningkatan Mana di sini
            if (playerStats != null)
            {
                playerStats.IncreaseMana(healingAmount);
            }

            // Kurangi jumlah manaPotion yang dimiliki
            currentManaPotionCount--;

            // Simpan jumlah item potion ke PlayerPrefs
            SavePotionCount();

            // Update tampilan jumlah potion
            UpdatePotionCountText();
        }
        else
        {
            Debug.Log("Tidak ada Mana Potion yang tersedia.");
        }
    }

    public bool CollectHpPotion()
    {
        // Check if there is space in the inventory for HP potions
        if (currentHpPotionCount < maxHpPotionCount)
        {
            // Increment the HP potion count
            currentHpPotionCount++;

            // Save the updated count
            SavePotionCount();

            // Update the UI text
            UpdatePotionCountText();

            return true; // Successfully collected HP potion
        }
        else
        {
            Debug.Log("Inventory for HP potions is full!");
            return false; // Unable to collect HP potion
        }
    }

    public bool CollectManaPotion()
    {
        // Check if there is space in the inventory for HP potions
        if (currentManaPotionCount < maxManaPotionCount)
        {
            // Increment the HP potion count
            currentManaPotionCount++;

            // Save the updated count
            SavePotionCount();

            // Update the UI text
            UpdatePotionCountText();

            return true; // Successfully collected HP potion
        }
        else
        {
            Debug.Log("Inventory for HP potions is full!");
            return false; // Unable to collect HP potion
        }
    }
}
