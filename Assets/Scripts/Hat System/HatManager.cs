using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField] HatSO[] hats;
    [SerializeField] Transform hatSpawnPosition;

    //Hat spawnedHat;

    public void SpawnHat(int id)
    {
        //Debug.Log($"Spawning hat at index {id}, there are hats up to index {hats.Length - 1}");

        //if (spawnedHat != null) Destroy(spawnedHat.gameObject);
        Hat spawnedHat = Instantiate(hats[id].Prefab, hatSpawnPosition.position, Quaternion.identity).GetComponent<Hat>();
        spawnedHat.hatSO = hats[id];
    }
}
