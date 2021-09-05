using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button PlayGameButton;
    public Button ExitButton;

    void Start()
    {
        PlayGameButton.onClick.AddListener(LoadGame);
        ExitButton.onClick.AddListener(ExitGame);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
