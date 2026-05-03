using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LevelCarousel : MonoBehaviour
{
    [Header("UI References")]
    public Image levelImage;
    public TextMeshProUGUI levelNameText;
    public RectTransform cardRect;

    [Header("Level Data")]
    public Sprite[] levelPreviews;   
    public string[] levelNames;      
    public string[] sceneNames;     

    private int currentIndex = 0;
    private bool isAnimating = false;

    private void OnEnable()
    {
        UpdateCard(); 
    }

    public void OnNextPressed()
    {
        if (isAnimating) return;
        currentIndex = (currentIndex + 1) % levelPreviews.Length;
        AnimateCard(fromRight: true);
    }

    public void OnPrevPressed()
    {
        if (isAnimating) return;
        currentIndex = (currentIndex - 1 + levelPreviews.Length) % levelPreviews.Length;
        AnimateCard(fromRight: false);
    }

    private void AnimateCard(bool fromRight)
    {
        isAnimating = true;
        float startX = fromRight ? 600f : -600f;

        cardRect.DOAnchorPosX(-startX, 0.25f).SetEase(Ease.InCubic).OnComplete(() => {
            UpdateCard();
            cardRect.anchoredPosition = new Vector2(startX, 0);
            cardRect.DOAnchorPosX(0, 0.3f).SetEase(Ease.OutCubic).OnComplete(() => {
                isAnimating = false;
            });
        });
    }

    private void UpdateCard()
    {
        levelImage.sprite = levelPreviews[currentIndex];
        levelNameText.text = levelNames[currentIndex];
    }

    public void OnStartLevelPressed()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(sceneNames[currentIndex]);
    }
}