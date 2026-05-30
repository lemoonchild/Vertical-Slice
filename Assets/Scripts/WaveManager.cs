using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    public WaveData[] waves;
    public Transform[] spawnPoints;

    [Header("Referencias")]
    public GameObject door;
    public StanceSelectionUI stanceUI;
    public Stance[] stanceOptions;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool allWavesDone = false;

    private void Start()
    {
        StartCoroutine(StartWave(currentWave));
    }

    private IEnumerator StartWave(int waveIndex)
    {
        yield return new WaitForSeconds(1f);
        Debug.Log($"Wave {waveIndex + 1} comenzando");

        foreach (EnemySpawnData spawnData in waves[waveIndex].enemies)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(spawnData.prefab, spawnPoint.position, Quaternion.identity);
            enemiesAlive++;

            // Suscribirse a la muerte del enemigo
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
                health.OnDeath += OnEnemyDied;
        }
    }

    private void OnEnemyDied()
    {
        enemiesAlive--;
        Debug.Log($"Enemigos vivos: {enemiesAlive}");

        if (enemiesAlive <= 0)
        {
            currentWave++;
            if (currentWave < waves.Length)
                StartCoroutine(StartWave(currentWave));
            else
                OnAllWavesDone();
        }
    }

    private void OnAllWavesDone()
    {
        Debug.Log("Todas las waves completadas");
        StanceSelectionUI.Instance.Show(stanceOptions, OpenDoor);
    }

    private void OpenDoor()
    {
        if (door != null)
            door.SetActive(false);
    }
}

[System.Serializable]
public class WaveData
{
    public EnemySpawnData[] enemies;
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject prefab;
}