using UnityEngine;

public class FeedingChair : MonoBehaviour
{
    [SerializeField] Transform chairPosition;

    Frog lastFrog;

    bool occupied;

    void OnTriggerEnter(Collider other)
    {
        if (!occupied && other.TryGetComponent(out Frog frog))
        {
            if (lastFrog != null && lastFrog != frog)
            {
                lastFrog.OnGrab -= FrogGrab;
            }

            frog.OnGrab += FrogGrab;

            if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();

            lastFrog = frog;
            frog.DisablePhysics();
            frog.OverridePosition = chairPosition;
            frog.ShouldOverridePosition = true;
            frog.stats.consumptionMultiplier = 0.7f;
        }
    }

    void FrogGrab()
    {
        lastFrog.EnablePhysics();
        lastFrog.ShouldOverridePosition = false;
        lastFrog.stats.consumptionMultiplier = 1f;

        occupied = false;
    }
}
