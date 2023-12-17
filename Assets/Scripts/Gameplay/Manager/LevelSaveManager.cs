using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class LevelSaveManager : MonoBehaviour
{
    public TextMeshProUGUI[] levelButtonsText;
    public Image[] levelButtonsImage;
    private List<string>[] savedLevels;
    public GameObject noSaveNotification;
    private bool isNoSaveNotificationVisible = false;
    public Button hideNoSaveButton;

    void Start()
    {
        savedLevels = new List<string>[3];
        for (int i = 0; i < savedLevels.Length; i++)
        {
            savedLevels[i] = new List<string>();
        }

        // Load data dari PlayerPrefs saat game dimulai
        LoadSavedLevels();

        // Update teks dan gambar tombol
        UpdateButtonContent();

        // Set up the hide button click event
        if (hideNoSaveButton != null)
        {
            hideNoSaveButton.onClick.AddListener(HideNoSaveNotificationButton);
            hideNoSaveButton.gameObject.SetActive(false); // Initially hide the hide button
        }
    }

    public void SaveLevel(int buttonIndex)
    {
        string currentLevel = SceneManager.GetActiveScene().name;

        // Check if the slot already contains a saved level
        if (savedLevels[buttonIndex].Count > 0)
        {
            // Remove the old save data for this slot
            savedLevels[buttonIndex].Clear();
        }

        // Add the new level to the slot
        savedLevels[buttonIndex].Add(currentLevel);

        // Save data to PlayerPrefs
        SaveLevelsToPlayerPrefs();

        // Update button content
        UpdateButtonContent();
    }

    private void UpdateButtonContent()
    {
        for (int i = 0; i < savedLevels.Length; i++)
        {
            if (savedLevels[i].Count > 0)
            {
                string levelName = savedLevels[i][savedLevels[i].Count - 1];

                // Set the text of the button
                levelButtonsText[i].text = levelName;

                // Find the corresponding image by name
                Sprite matchingSprite = Resources.Load<Sprite>("Images/" + levelName);

                if (matchingSprite != null)
                {
                    // Set the sprite of the Image component
                    levelButtonsImage[i].sprite = matchingSprite;

                    // Show the Image component
                    levelButtonsImage[i].gameObject.SetActive(true);
                }
                else
                {
                    // Hide the Image component if no matching image is found
                    levelButtonsImage[i].gameObject.SetActive(false);
                }
            }
            else
            {
                levelButtonsText[i].text = "Empty";

                // Hide the Image component
                levelButtonsImage[i].gameObject.SetActive(false);
            }
        }
    }

    public void LoadLevel(int buttonIndex)
    {
        if (savedLevels[buttonIndex].Count > 0)
        {
            string levelToLoad = savedLevels[buttonIndex][savedLevels[buttonIndex].Count - 1];
            SceneManager.LoadScene(levelToLoad);
        }
    }

    // Fungsi untuk menyimpan data ke PlayerPrefs
    private void SaveLevelsToPlayerPrefs()
    {
        for (int i = 0; i < savedLevels.Length; i++)
        {
            string levelsString = string.Join(",", savedLevels[i].ToArray());
            PlayerPrefs.SetString("SavedLevels_" + i, levelsString);
        }
    }

    // Fungsi untuk memuat data dari PlayerPrefs
    private void LoadSavedLevels()
    {
        for (int i = 0; i < savedLevels.Length; i++)
        {
            string levelsString = PlayerPrefs.GetString("SavedLevels_" + i, "");
            if (!string.IsNullOrEmpty(levelsString))
            {
                savedLevels[i] = new List<string>(levelsString.Split(','));
            }
        }
    }

    public void Continue()
    {
        // Cari indeks penyimpanan terakhir yang tidak kosong
        int lastIndex = -1;

        for (int i = 0; i < savedLevels.Length; i++)
        {
            if (savedLevels[i].Count > 0)
            {
                lastIndex = i;
            }
        }

        // Jika ada penyimpanan yang tersedia, load level
        if (lastIndex != -1)
        {
            HideNoSaveNotification(); // Hide the notification if there is a saved game
            LoadLevel(lastIndex);
        }
        else
        {
            // Tampilkan pemberitahuan jika tidak ada penyimpanan
            ShowNoSaveNotification();
        }
    }

    // Menampilkan pemberitahuan jika tidak ada penyimpanan
     private void ShowNoSaveNotification()
    {
        if (noSaveNotification != null && !isNoSaveNotificationVisible)
        {
            noSaveNotification.SetActive(true);
            isNoSaveNotificationVisible = true;

            // Show the hide button
            if (hideNoSaveButton != null)
            {
                hideNoSaveButton.gameObject.SetActive(true);
            }
        }
    }


    // Menyembunyikan pemberitahuan jika ada penyimpanan
    private void HideNoSaveNotification()
    {
        if (noSaveNotification != null && isNoSaveNotificationVisible)
        {
            noSaveNotification.SetActive(false);
            isNoSaveNotificationVisible = false;

            // Hide the hide button
            if (hideNoSaveButton != null)
            {
                hideNoSaveButton.gameObject.SetActive(false);
            }
        }
    }
    // Memperbarui tampilan pemberitahuan berdasarkan keberadaan penyimpanan
    private void UpdateNoSaveNotification()
    {
        if (savedLevels.Any(levelList => levelList.Count > 0))
        {
            HideNoSaveNotification();
        }
        else
        {
            ShowNoSaveNotification();
        }
    }
    private void HideNoSaveNotificationButton()
    {
        HideNoSaveNotification();
    }
    public bool HasSaveData()
    {
        // Check if any of the save slots has data
        return savedLevels.Any(levelList => levelList.Count > 0);
    }

    
}