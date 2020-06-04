using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

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
    }
}
