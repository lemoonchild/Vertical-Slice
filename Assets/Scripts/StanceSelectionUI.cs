using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StanceSelectionUI : MonoBehaviour
{
    public static StanceSelectionUI Instance;

    [Header("Panel")]
    public GameObject panel;

    [Header("Botones")]
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public TextMeshProUGUI txt1;
    public TextMeshProUGUI txt2;
    public TextMeshProUGUI txt3;

    private Stance[] currentOptions;
    private System.Action onSelected;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
    }

    public void Show(Stance[] options, System.Action callback)
    {
        Debug.Log("Show() llamado");
        currentOptions = options;
        onSelected = callback;

        txt1.text = GetStanceName(options[0]);
        txt2.text = GetStanceName(options[1]);
        txt3.text = options.Length > 2 ? GetStanceName(options[2]) : "";
        btn3.gameObject.SetActive(options.Length > 2);

        btn1.onClick.RemoveAllListeners();
        btn2.onClick.RemoveAllListeners();
        btn3.onClick.RemoveAllListeners();

        btn1.onClick.AddListener(() => Select(0));
        btn2.onClick.AddListener(() => Select(1));
        btn3.onClick.AddListener(() => Select(2));

        panel.SetActive(true);
        Debug.Log($"Panel activo: {panel.activeSelf}, padre activo: {panel.transform.parent.gameObject.activeSelf}");
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Select(int index)
    {
        StanceSystem.Instance.SetStance(currentOptions[index]);
        panel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        onSelected?.Invoke();
    }

    private string GetStanceName(Stance stance)
    {
        return stance switch
        {
            Stance.Damage => "Más daño",
            Stance.Healing => "Curación",
            Stance.Invisibility => "Invisibilidad",
            Stance.Speed => "Velocidad",
            _ => ""
        };
    }
}