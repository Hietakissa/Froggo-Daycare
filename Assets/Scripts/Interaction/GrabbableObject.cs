using System;
using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    public event Action OnStartGrab;
    public event Action OnStopGrab;

    public void StartGrab()
    {
        //GrabbingController.Instance.GrabObject();
        OnStartGrab?.Invoke();
    }

    public void StopGrab()
    {
        //GrabbingController.Instance.UnGrabObject();
        OnStopGrab?.Invoke();
    }
}
