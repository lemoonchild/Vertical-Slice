using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Referencias")]
    public Image fillBar;
    public Transform target;
    public Vector3 offset = new Vector3(0, 2.5f, 0);

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }

    public void UpdateBar(float current, float max)
    {
        if (fillBar != null)
            fillBar.fillAmount = current / max;
    }
}