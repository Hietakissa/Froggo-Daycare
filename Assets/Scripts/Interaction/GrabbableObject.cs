using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    public void StartGrab()
    {
        PlayerData.grabbingObject = true;

        GrabbingController.Instance.GrabObject();
    }

    public void StopGrab()
    {
        PlayerData.grabbingObject = false;

        GrabbingController.Instance.UnGrabObject();
    }
}
