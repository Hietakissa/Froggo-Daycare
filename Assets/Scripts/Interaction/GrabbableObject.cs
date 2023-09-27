using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    public void StartGrab()
    {
        GrabbingController.Instance.GrabObject();
    }

    public void StopGrab()
    {
        GrabbingController.Instance.UnGrabObject();
    }
}
