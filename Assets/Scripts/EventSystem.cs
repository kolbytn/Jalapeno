using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSystem : MonoBehaviour
{
    public GameObject gameMenu;
    public Button saveButton;
    public Button loadButton;
    public Button exitButton;

    private bool isVisible = false;

    void Start()
    {
        Button saveBtn = saveButton.GetComponent<Button>();
        saveBtn.onClick.AddListener(SaveGame);
        Button loadBtn = loadButton.GetComponent<Button>();
        loadBtn.onClick.AddListener(LoadGame);
        Button exitBtn = exitButton.GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitGame);
    }

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

    void SaveGame()
    {
        WorldController.Instance.SaveGameToFile("test.json");
        Debug.Log("Save Game");
    }

    void LoadGame()
    {
        WorldController.Instance.LoadGameFromFile("test.json");
        Debug.Log("Load Game");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}
