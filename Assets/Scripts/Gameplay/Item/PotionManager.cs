using System.Collections;
using TMPro;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    public int maxHpPotionCount = 5;
    public int maxManaPotionCount = 5;
    public TextMeshProUGUI hpPotionCountText;
    public TextMeshProUGUI manaPotionCountText;

    private int currentHpPotionCount = 0;
    private int currentManaPotionCount = 0;

    void Start()
    {
        // Muat jumlah item potion dari PlayerPrefs saat memulai
        LoadPotionCount();
        UpdatePotionCountText();
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
                // Tambah satu ke jumlah potion yang dimiliki
                currentHpPotionCount++;

                // Hancurkan objek potion yang diambil
                Destroy(gameObject);

                // Simpan jumlah item potion ke PlayerPrefs
                SavePotionCount();

                // Update tampilan jumlah potion
                UpdatePotionCountText();
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
            // Gunakan hpPotion, contoh: tambahkan logika penambahan HP di sini
            Debug.Log("Menggunakan HP Potion");

            // Kurangi jumlah hpPotion yang dimiliki
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
            // Gunakan manaPotion, contoh: tambahkan logika penambahan Mana di sini
            Debug.Log("Menggunakan Mana Potion");

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
}
