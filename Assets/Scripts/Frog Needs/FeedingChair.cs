using UnityEngine;

public class FeedingChair : MonoBehaviour
{
    [SerializeField] Transform chairPosition;

    Frog lastFrog;

    bool occupied;

    void OnTriggerEnter(Collider other)
    {
        if (!occupied/* && other.TryGetComponent(out Frog frog)*/ && GameManager.TryGetFrog(other, out Frog frog) && (!frog.ShouldOverridePosition && frog.TimeNotOverriddenPositionFor > 2f))
        {
            if (lastFrog != null && lastFrog != frog)
            {
                lastFrog.OnGrab -= FrogGrab;
            }

            frog.OnGrab += FrogGrab;

            if (PlayerData.lastGrabObject == frog.gameObject)
            {
                //Debug.Log("Feeding chair ungrabbed object");
                GrabbingController.Instance.UnGrabObject();
            }

            lastFrog = frog;
            frog.DisablePhysics();
            frog.OverridePosition = chairPosition;
            frog.ShouldOverridePosition = true;
            frog.stats.consumptionMultiplier = 0.7f;

            occupied = true;

            //Debug.Log("Frog entered feeding chair");
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
