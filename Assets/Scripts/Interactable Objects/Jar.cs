using UnityEngine;

public class Jar : MonoBehaviour, IInteractable
{
    [SerializeField] Transform spawnPos;

    public void Interact()
    {
        FrogSpawner.Instance.SpawnFrog(spawnPos);
        LevelManager.Instance.AddXP(100);

        if (PlayerData.lastGrabObject == gameObject)
        {
            Debug.Log("Jar ungrabbed object");
            GrabbingController.Instance.UnGrabObject();
        }
        Destroy(gameObject);
    }
}
