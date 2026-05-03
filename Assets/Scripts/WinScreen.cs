using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public GameObject returnButton;

    private void Start()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        winText.alpha = 0f;
        winText.DOFade(1f, 1f).SetUpdate(true);

        returnButton.SetActive(false);
        DOVirtual.DelayedCall(1.5f, () => {
            returnButton.SetActive(true);
        }).SetUpdate(true);
    }

    public void OnContinuePressed()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.UnloadSceneAsync("WinScreen");
        SceneManager.LoadScene("MainMenu");
    }
}