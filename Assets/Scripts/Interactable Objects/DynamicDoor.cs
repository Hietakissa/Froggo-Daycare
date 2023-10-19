using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class DynamicDoor : MonoBehaviour, IGrabbable
{
    [SerializeField] public Orientation orientation;
    [SerializeField] public bool invertMaxAngle;

    [SerializeField] int snapShutAngle = 1;
    [SerializeField] float snapShutMinVelocity = 2f;

    [SerializeField] float closingAssistForce = 35;

    public event Action onDoorClosed;

    public bool IsClosed = true;
    Quaternion closedRot;
    Rigidbody rb;
    bool isGrabbed;

    float DoorAngle => Quaternion.Angle(closedRot, transform.rotation);

    //TMP_Text text;

    Vector3 orientationAxis;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        closedRot = transform.rotation;

        orientationAxis = GetOrientationAxis();
        //text = transform.parent.GetComponentInChildren<TMP_Text>();
    }
    
    void Start()
    {
        SnapShut();
    }

    void Update()
    {
        //text.text = $"Grabbed: {isGrabbed}\nClosed: {IsClosed}";

        if (isGrabbed) return;
        
        CheckShut();
    }

    void FixedUpdate()
    {
        if (isGrabbed || IsClosed) return;

        rb.AddTorque(transform.InverseTransformDirection(orientationAxis) * closingAssistForce * Time.deltaTime, ForceMode.Acceleration);
    }

    void CheckShut()
    {
        if (DoorAngle <= snapShutAngle)
        {
            if ((rb.velocity != Vector3.zero) && rb.angularVelocity.magnitude <= snapShutMinVelocity) SnapShut();
        }
        else IsClosed = false;
    }

    void SnapShut()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = closedRot;

        IsClosed = true;

        onDoorClosed?.Invoke();
    }

    public void StartGrab()
    {
        isGrabbed = true;
    }

    public void StopGrab()
    {
        isGrabbed = false;
    }

    public Vector3 GetOrientationAxis()
    {
        switch (orientation)
        {
            case Orientation.Horizontal: return Vector3.up;
            case Orientation.Vertical: return Vector3.right;
            default: return Vector3.zero;
        }
    }
}

public enum Orientation
{
    Horizontal,
    Vertical
}
