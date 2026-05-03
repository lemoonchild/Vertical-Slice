using UnityEngine;
using UnityEngine.InputSystem;

public class CollectItem : MonoBehaviour
{
    [Header("Referencias")]
    public PuzzleManager puzzleManager; 

    [Header("Configuración")]
    public string itemName = "Llave";  

    private bool playerNearby = false;
    private bool collected = false;

    [Header("UI")]
    public GameObject interactText; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

    private void Update()
    {
        if (playerNearby && !collected &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Collect();
        }
    }

    private void Collect()
    {
        collected = true;
        interactText.SetActive(false);
        puzzleManager.ItemCollected();
        gameObject.SetActive(false); 
    }
}