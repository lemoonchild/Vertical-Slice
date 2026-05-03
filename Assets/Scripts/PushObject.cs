using UnityEngine;
using DG.Tweening;

public class PushObject : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pushObject;     
    public GameObject doorBlock;     

    [Header("Configuración")]
    public float blockDropDistance = 5f;

    private bool solved = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!solved && other.gameObject == pushObject)
        {
            solved = true;
            OnPuzzleSolved();
        }
    }

    private void OnPuzzleSolved()
    {
        Rigidbody rb = pushObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Vector3 targetPos = doorBlock.transform.position;
        targetPos.y -= blockDropDistance;
        doorBlock.transform.DOMove(targetPos, 0.8f)
            .SetDelay(0.3f)
            .SetEase(Ease.InCubic)
            .OnComplete(() => {
                doorBlock.SetActive(false);
            });
    }
}