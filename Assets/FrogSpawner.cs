using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    [SerializeField] GameObject frogPrefab;
    [SerializeField] float frogSpawnDelay;
    float frogSpawnTime;

    [SerializeField] bool spawnFrogs;

    void Update()
    {
        if (!spawnFrogs) return;

        frogSpawnTime += Time.deltaTime;

        if (frogSpawnTime >= frogSpawnDelay)
        {
            frogSpawnTime -= frogSpawnDelay;
            SpawnFrog();
        }
    }

    void SpawnFrog()
    {
        Instantiate(frogPrefab, transform.position, transform.rotation);
    }
}
