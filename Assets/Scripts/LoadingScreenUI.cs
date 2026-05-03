using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoadingScreenUI : MonoBehaviour
{
    [Header("Spinning Icon")]
    public RectTransform spinningIcon;
    public float rotationSpeed = 200f;

    [Header("Tips")]
    public TextMeshProUGUI tipText;
    public float tipDisplayTime = 2f;

    private string[] tips = {
        "Las trampas suelen estar donde menos lo esperas...",
        "Explora cada rincón, los secretos se esconden en la oscuridad.",
        "Preparando las sombras para ti...",
        "Cada paso puede ser el último, avanza con cautela."
    };

    private int currentTipIndex = 0;
    private float tipTimer = 0f;
    private bool fadingOut = false;

    private void Start()
    {
        currentTipIndex = Random.Range(0, tips.Length);
        tipText.text = tips[currentTipIndex];
        tipText.alpha = 1f;
    }

    private void Update()
    {
        // Rotar ícono constantemente
        spinningIcon.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);

        // Ciclo de tips
        tipTimer += Time.deltaTime;
        if (tipTimer >= tipDisplayTime && !fadingOut)
        {
            fadingOut = true;
            tipText.DOFade(0f, 0.5f).OnComplete(() => {
                int newIndex;
                do {
                    newIndex = Random.Range(0, tips.Length);
                } while (newIndex == currentTipIndex);
                
                currentTipIndex = newIndex;
                tipText.text = tips[currentTipIndex];
                tipText.DOFade(1f, 0.5f).OnComplete(() => {
                    fadingOut = false;
                    tipTimer = 0f;
                });
            });
        }
    }
}