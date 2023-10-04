using UnityEngine;

public class Hat : MonoBehaviour, IGrabbable
{
    bool isGrabbed;

    public void StartGrab()
    {
        isGrabbed = true;
    }

    public void StopGrab()
    {
        isGrabbed = false;
    }
}
