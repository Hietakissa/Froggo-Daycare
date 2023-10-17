using System;
using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    public event Action OnStartGrab;
    public event Action OnStopGrab;

    Vector3 startPos;
    Quaternion startRot;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Start()
    {
        PauseManager.Instance.RegisterRigidbody(rb);
    }

    void Update()
    {
        if (transform.position.y <= -5f)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = startPos;
            rb.rotation = startRot;
        }
    }

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
