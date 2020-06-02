using UnityEngine;
using UnityEngine.UI;

public class EventSystem : MonoBehaviour {

    // Set in editor
    public GameObject GameMenu;
    public Button SaveButton;
    public Button LoadButton;
    public Button ExitButton;

    private bool isVisible = false;

    void Start() {

        Button saveBtn = SaveButton.GetComponent<Button>();
        saveBtn.onClick.AddListener(SaveGame);
        Button loadBtn = LoadButton.GetComponent<Button>();
        loadBtn.onClick.AddListener(LoadGame);
        Button exitBtn = ExitButton.GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitGame);
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            isVisible = !isVisible;
            if (isVisible)
                Debug.Log("Is Visible");
            else
                Debug.Log("Is not Visible");
        }

        GameMenu.SetActive(isVisible);
    }

    void SaveGame() {
        WorldController.Instance.SaveGameToFile("test.json");
        Debug.Log("Save Game");
    }

    void LoadGame() {
        WorldController.Instance.LoadGameFromFile("test.json");
        Debug.Log("Load Game");
    }

    void ExitGame() {
        Application.Quit();
    }
}
