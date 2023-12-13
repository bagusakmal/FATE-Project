using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ListFragment : MonoBehaviour
{
    public PlayerCombatController playerCombatController;

    [System.Serializable]
    public struct ItemData
    {
        public Sprite img;
        public string itemName;
        public string itemDescription;
    }

    public ItemData[] itemList;
    public Sprite defaultImage;
    public Image[] itemImages;
    public List<int> collectedItems = new List<int>();
    public PlayerStats playerStats; // Add reference to PlayerStats

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
            if (i < collectedItems.Count)
            {
                int itemIndex = collectedItems[i];

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
        foreach (int itemIndex in collectedItems)
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

    public void AddItemToCollectedList(int itemIndex)
    {
        if (!collectedItems.Contains(itemIndex))
        {
            collectedItems.Add(itemIndex);
            SaveCollectedItems();
            DisplayCollectedItems();
            ApplyItemEffect(itemIndex); // Apply item effect when added to the list
        }
    }

    public void DeleteAllItems()
    {
        collectedItems.Clear();
        SaveCollectedItems();
        DisplayCollectedItems();
    }

    private void SaveCollectedItems()
    {
        string key = "CollectedItems";
        string data = string.Join(",", collectedItems.ConvertAll(i => i.ToString()).ToArray());
        PlayerPrefs.SetString(key, data);
    }

    public void LoadCollectedItems()
    {
        string key = "CollectedItems";
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            string[] indices = data.Split(',');
            collectedItems = new List<int>(System.Array.ConvertAll(indices, int.Parse));
        }
    }

    public bool HasItemBeenCollected(int itemIndex)
    {
        return collectedItems.Contains(itemIndex);
    }
}