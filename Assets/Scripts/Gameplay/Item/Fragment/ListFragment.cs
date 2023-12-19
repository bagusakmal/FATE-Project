using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ListFragment : MonoBehaviour
{
    public PlayerCombatController playerCombatController;

    [System.Serializable]
    public struct FragmentData
    {
        public Sprite img;
        public string itemName;
        public string itemDescription;
    }

    public FragmentData[] itemList;
    public Sprite defaultImage;
    public Image[] itemImages;
    public List<int> collectedFragment = new List<int>();
    public PlayerStats playerStats; // Add reference to PlayerStats
    public Canvas itemInfoCanvas; 
    public Image itemInfoImage;   
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText; 
    

    void Start()
    {
        LoadCollectedItems();

        if (itemImages.Length != itemList.Length)
        {
            Debug.LogError("ItemImages and ItemList lengths do not match.");
            return;
        }

        DisplayCollectedItems();
        ApplyCollectedItemsEffects(); // Call the function to apply collected item effects
    }

    public void DisplayCollectedItems()
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (i < collectedFragment.Count)
            {
                int itemIndex = collectedFragment[i];

                if (itemIndex < itemList.Length)
                {
                    itemImages[i].sprite = itemList[itemIndex].img;
                }
            }
            else
            {
                itemImages[i].sprite = defaultImage;
            }
        }
    }

    void ApplyCollectedItemsEffects()
    {
        // Iterate through the collected items and apply their effects to PlayerStats
        foreach (int itemIndex in collectedFragment)
        {
            if (itemIndex >= 0 && itemIndex < itemList.Length)
            {
                ApplyItemEffect(itemIndex);
            }
        }
    }

    void ApplyItemEffect(int itemIndex)
    {
        switch (itemIndex)
        {
            case 0:
                playerStats.IncreaseMaxHealth(20f);
                break;
            case 1:
                playerStats.IncreaseMaxHealth(20f);
                break;
            case 2:
                if (playerCombatController != null)
                {
                    playerCombatController.IncreaseAttack1Damage(10f);
                }
                break;
            case 3:
                if (playerCombatController != null)
                {
                    playerCombatController.IncreaseAttack1Damage(10f);
                }
                break;
            case 4:
                playerStats.IncreaseMaxMana(10f);
            break;
             case 5:
                playerStats.IncreaseMaxMana(10f);
            break;
        // Add other cases as needed
        default:
            break;
        }
    }

    public void DeleteAllItems()
    {
        collectedFragment.Clear();
        SaveCollectedItems();
        DisplayCollectedItems();
    }

    private void SaveCollectedItems()
    {
        string key = "CollectedFragment";
        string data = string.Join(",", collectedFragment.ConvertAll(i => i.ToString()).ToArray());
        PlayerPrefs.SetString(key, data);
    }

    public void LoadCollectedItems()
    {
        string key = "CollectedFragment";
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            string[] indices = data.Split(',');
            collectedFragment = new List<int>(System.Array.ConvertAll(indices, int.Parse));
        }
    }

    public bool HasItemBeenCollected(int itemIndex)
    {
        return collectedFragment.Contains(itemIndex);
    }

    public void AddItemToCollectedList(int itemIndex)
    {
        if (!collectedFragment.Contains(itemIndex))
        {
            collectedFragment.Add(itemIndex);
            SaveCollectedItems();
            DisplayCollectedItems();
            ApplyItemEffect(itemIndex);

            StartCoroutine(ShowItemInfoForDuration(itemIndex, 3f));
        }
    }

    IEnumerator ShowItemInfoForDuration(int itemIndex, float duration)
    {
        if (itemIndex >= 0 && itemIndex < itemList.Length)
        {
            itemInfoImage.sprite = itemList[itemIndex].img;
            itemNameText.text = itemList[itemIndex].itemName;
            itemDescriptionText.text = itemList[itemIndex].itemDescription;
        }

        itemInfoCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        itemInfoCanvas.gameObject.SetActive(false);
    }


}