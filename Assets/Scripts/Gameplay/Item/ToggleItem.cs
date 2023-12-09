using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleItem : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    
    private ListItem listItemScript;

    private void Start()
    {
        listItemScript = GetComponent<ListItem>();

        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((value) => OnToggleValueChanged(toggle, value));
        }
    }

    private void OnToggleValueChanged(Toggle toggle, bool value)
{
    if (value)
    {
        int toggleIndex = toggle.transform.GetSiblingIndex();

        int collectedItemIndex = -1;
        if (toggleIndex < listItemScript.collectedItems.Count)
        {
            collectedItemIndex = listItemScript.collectedItems[toggleIndex];
        }

        if (collectedItemIndex != -1)
        {
            DisplayItemInfo(collectedItemIndex);
        }
        else
        {
            ClearItemInfo();
        }
    }
}


    private void DisplayItemInfo(int itemIndex)
    {
        itemNameText.text = listItemScript.itemList[itemIndex].itemName;
        itemDescriptionText.text = listItemScript.itemList[itemIndex].itemDescription;
    }

    private void ClearItemInfo()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }
}