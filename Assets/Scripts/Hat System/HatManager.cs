using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField] HatSO[] hats;
    [SerializeField] Transform hatSpawnPosition;

    public void SpawnHat(int id)
    {
        Instantiate(hats[id].Prefab, hatSpawnPosition.position, Quaternion.identity);
    }
}
