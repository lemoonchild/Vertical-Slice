using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Waves")]
    public WaveData[] waves;
    public Transform[] spawnPoints;

    [Header("Referencias")]
    public GameObject door;
    public Stance[] stanceOptions;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    private void Start()
    {
        StartCoroutine(StartWave(currentWave));
    }

    private IEnumerator StartWave(int waveIndex)
    {
        yield return new WaitForSeconds(1f);

        foreach (EnemySpawnData spawnData in waves[waveIndex].enemies)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(spawnData.prefab, spawnPoint.position, Quaternion.identity);
            enemiesAlive++;

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
                health.OnDeath += OnEnemyDied;
        }
    }

    private void OnEnemyDied()
    {
        enemiesAlive--;

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