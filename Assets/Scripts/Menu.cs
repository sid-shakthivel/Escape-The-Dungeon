using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button PlayGameButton;

    void Start()
    {
        PlayGameButton.onClick.AddListener(LoadGame);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
