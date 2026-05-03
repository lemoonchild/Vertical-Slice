using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class LeverInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform leverHandle;      
    public GameObject doorBlock;       

    [Header("Configuración")]
    public float leverRotation = 60f;  
    public float blockDropDistance = 3f; 

    [Header("UI")]
    public GameObject interactText; 

    private bool playerNearby = false;
    private bool activated = false;

    private void Start()
    {
        interactText.SetActive(false); 
    }

    private void Update()
    {
        if (playerNearby && !activated)
        {
            if (UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                ActivateLever();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            interactText.SetActive(false);
        }
    }

    private void ActivateLever()
    {
        activated = true;
        interactText.SetActive(false);

        leverHandle.DOLocalRotate(
            new Vector3(leverRotation, 0f, 0f),
            0.4f
        ).SetEase(Ease.OutBack);

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