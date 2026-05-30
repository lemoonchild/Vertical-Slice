using UnityEngine;
using UnityEngine.SceneManagement;

public class CatInteract : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene("WinScreen");
    }
}