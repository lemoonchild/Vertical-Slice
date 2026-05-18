using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Enemigos")]
    public GameObject[] enemies;

    [Header("Puerta")]
    public GameObject door;

    [Header("Posiciones disponibles")]
    public Stance[] stanceOptions;

    private bool roomCleared = false;

    private void Update()
    {
        if (roomCleared) return;

        bool allDead = true;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            roomCleared = true;
            OnRoomCleared();
        }
    }

    private void OnRoomCleared()
    {
        Debug.Log("Cuarto limpiado, mostrando panel");
        if (StanceSelectionUI.Instance == null)
        {
            Debug.LogError("StanceSelectionUI.Instance es null");
            return;
        }
        Debug.Log("Mostrando panel de posiciones");
        StanceSelectionUI.Instance.Show(stanceOptions, OpenDoor);
    }

    private void OpenDoor()
    {
        if (door != null)
            door.SetActive(false);
    }
}