using UnityEngine;
using System;

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude / Time.fixedDeltaTime >= 0.3f)
        {
            SoundManager.Instance.PlayGenericImpact(transform.position);
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
