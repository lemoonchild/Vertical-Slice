using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        List<int> availablePoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
            availablePoints.Add(i);

        foreach (EnemySpawnData spawnData in waves[waveIndex].enemies)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform spawnPoint = spawnPoints[availablePoints[randomIndex]];
            availablePoints.RemoveAt(randomIndex);
            
            if (availablePoints.Count == 0)
                for (int i = 0; i < spawnPoints.Length; i++)
                    availablePoints.Add(i);

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