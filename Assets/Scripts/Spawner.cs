using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public GameObject powerUpPrefab;

    public float pipeSpawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 2f;

    public float minPowerUpSpawnRate = 5f; // Minimum time between power-up spawns
    public float maxPowerUpSpawnRate = 15f; // Maximum time between power-up spawns

    private float nextPowerUpSpawnTime;

    private void OnEnable()
    {
        InvokeRepeating(nameof(SpawnPipe), 0f, pipeSpawnRate);
        nextPowerUpSpawnTime = Time.time + Random.Range(minPowerUpSpawnRate, maxPowerUpSpawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnPipe));
    }

    private void Update()
    {
        if (Time.time >= nextPowerUpSpawnTime)
        {
            SpawnPowerUp();
            nextPowerUpSpawnTime = Time.time + Random.Range(minPowerUpSpawnRate, maxPowerUpSpawnRate);
        }
    }

    private void SpawnPipe()
    {
        GameObject pipes = Instantiate(pipePrefab, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }

    private void SpawnPowerUp()
    {
        // Spawn power-up at a random position between two pipes
        float yPos = Random.Range(minHeight, maxHeight);
        GameObject powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        powerUp.transform.position += Vector3.up * yPos;
    }
}
