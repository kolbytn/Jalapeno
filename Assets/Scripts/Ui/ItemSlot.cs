using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image SlotIcon;
    public Image ItemIcon;
    public Text Count;

    void Start() {
        // ItemIcon.enabled = false;
    }

    public void SetItem(Item item) {
        ItemIcon.enabled = true;
        ItemIcon.sprite = item.GetIconSprite();
        Count.text = item.Quantity.ToString();
    }

    public void ClearIcon() {
        ItemIcon.sprite = null;
        ItemIcon.enabled = false;
        Count.text = "";
    }

    public void Highlight() {
        SlotIcon.color = new Color(255, 255, 0, 255);
    }

    public void UnHighlight() {
        SlotIcon.color = new Color(255, 255, 255, 255);
    }
}
