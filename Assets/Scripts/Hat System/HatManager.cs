using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField] HatSO[] hats;
    [SerializeField] Transform hatSpawnPosition;

    Hat spawnedHat;

    public void SpawnHat(int id)
    {
        if (spawnedHat != null) Destroy(spawnedHat.gameObject);
        spawnedHat = Instantiate(hats[id].Prefab, hatSpawnPosition.position, Quaternion.identity).GetComponent<Hat>();
        spawnedHat.hatSO = hats[id];
    }
}
