using UnityEngine;

public class Jar : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        FrogSpawner.Instance.SpawnFrog(transform);

        if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();
        Destroy(gameObject);
    }
}
