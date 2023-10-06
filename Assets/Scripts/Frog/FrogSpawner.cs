using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    public static FrogSpawner Instance;

    [SerializeField] GameObject frogPrefab;
    [SerializeField] GameObject jarPrefab;
    [SerializeField] float frogSpawnDelay;
    float frogSpawnTime;

    [SerializeField] bool spawnFrogs;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!spawnFrogs) return;

        frogSpawnTime += Time.deltaTime;

        if (frogSpawnTime >= frogSpawnDelay)
        {
            frogSpawnTime -= frogSpawnDelay;
            //SpawnFrog();
            SpawnJar();
        }
    }

    void SpawnJar()
    {
        Instantiate(jarPrefab, transform.position, transform.rotation);
    }

    public void SpawnFrog(Transform jarTransform)
    {
        Instantiate(frogPrefab, jarTransform.position, jarTransform.rotation);
    }
}
