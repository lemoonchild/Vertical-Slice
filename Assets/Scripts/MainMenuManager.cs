using UnityEngine;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject carouselPanel;

    private void Start()
    {
        carouselPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnPlayPressed()
    {
        // LoadingManager.Instance.LoadScene("Level1");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level01");
    }

    public void OnSelectLevelPressed()
    {
        mainMenuPanel.transform.DOScale(0f, 0.3f).OnComplete(() => {
            mainMenuPanel.SetActive(false);
            carouselPanel.SetActive(true);
            carouselPanel.transform.localScale = Vector3.zero;
            carouselPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        });
    }

    public void OnBackToMenu()
    {
        carouselPanel.transform.DOScale(0f, 0.3f).OnComplete(() => {
            carouselPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            mainMenuPanel.transform.localScale = Vector3.zero;
            mainMenuPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        });
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}