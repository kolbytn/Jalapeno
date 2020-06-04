using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    public Image ItemIcon;

    void Start() {
        // ItemIcon.enabled = false;
    }

    public void SetIcon(Sprite sprite) {
        ItemIcon.enabled = true;
        ItemIcon.sprite = sprite;
    }

    public void ClearIcon() {
        ItemIcon.sprite = null;
        ItemIcon.enabled = false;
    }
}
