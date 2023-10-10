using UnityEngine;

public class Jar : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        FrogSpawner.Instance.SpawnFrog(transform);
        LevelManager.Instance.AddXP(100);

        if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();
        Destroy(gameObject);
    }
}
