using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public abstract class Appliance : MonoBehaviour
{
    [SerializeField] DynamicDoor dynamicDoor;
    [SerializeField] Orientation doorOrientation;

    void OnValidate()
    {
        if (Application.isPlaying) return;

        HingeJoint joint = GetComponent<HingeJoint>();
        Rigidbody doorRB = dynamicDoor.GetComponent<Rigidbody>();

        switch (doorOrientation)
        {
            case Orientation.Horizontal:
                joint.axis = Vector3.up;
                break;
            case Orientation.Vertical:
                joint.axis = Vector3.right;
            break;
        }

        joint.anchor = dynamicDoor.transform.localPosition;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector3.zero;

        doorRB.interpolation = RigidbodyInterpolation.Interpolate;
        doorRB.useGravity = false;
        doorRB.drag = 7f;
    }
}

enum Orientation
{
    Horizontal,
    Vertical
}