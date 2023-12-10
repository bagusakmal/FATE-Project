using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ListItem : MonoBehaviour
{
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

    void Start()
    {
        LoadCollectedItems();

        if (itemImages.Length != itemList.Length)
        {
            return;
        }

        DisplayCollectedItems();
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

    public void AddItemToCollectedList(int itemIndex)
    {
        if (!collectedItems.Contains(itemIndex))
        {
            collectedItems.Add(itemIndex);
            SaveCollectedItems();
            DisplayCollectedItems();
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
