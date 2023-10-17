using UnityEngine;

public class Jar : MonoBehaviour, IInteractable
{
    [SerializeField] Transform spawnPos;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PauseManager.Instance.RegisterRigidbody(rb);
    }

    public void Interact()
    {
        FrogSpawner.Instance.SpawnFrog(spawnPos);
        LevelManager.Instance.AddXP(100);

        if (PlayerData.lastGrabObject == gameObject)
        {
            Debug.Log("Jar ungrabbed object");
            GrabbingController.Instance.UnGrabObject();
        }

        PauseManager.Instance.UnregisterRigidbody(rb);
        Destroy(gameObject);
    }
}
