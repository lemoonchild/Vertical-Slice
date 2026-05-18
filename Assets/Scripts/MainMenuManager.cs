using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mainMenuPanel.SetActive(true);
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Level01");
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}