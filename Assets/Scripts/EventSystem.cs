using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public GameObject gameMenu;

    private bool isVisible = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isVisible = !isVisible;
            if (isVisible)
                Debug.Log("Is Visible");
            else
                Debug.Log("Is not Visible");
        }

        gameMenu.SetActive(isVisible);
    }
}
