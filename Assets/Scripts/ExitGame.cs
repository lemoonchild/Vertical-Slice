using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public string nextSceneName;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;

            if (nextSceneName == "MainMenu")
            {
                SceneManager.LoadSceneAsync("WinScreen", LoadSceneMode.Additive);
            }
        
        }
    }
}