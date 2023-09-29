using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public abstract class Appliance : MonoBehaviour
{
    [SerializeField] protected DynamicDoor dynamicDoor;

    void OnValidate()
    {
        if (Application.isPlaying) return;

        ValidateVariables();
    }

    [ContextMenu("Validate")]
    void ValidateVariables()
    {
        HingeJoint joint = GetComponent<HingeJoint>();
        Rigidbody doorRB = dynamicDoor.GetComponent<Rigidbody>();

        joint.connectedBody = doorRB;

        joint.axis = dynamicDoor.GetOrientationAxis();

        joint.anchor = dynamicDoor.transform.localPosition;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector3.zero;

        JointLimits limits = new JointLimits();
        limits.min = 0;
        limits.max = 110;

        joint.useLimits = true;
        joint.limits = limits;

        doorRB.interpolation = RigidbodyInterpolation.Interpolate;
        doorRB.useGravity = false;
        doorRB.drag = 0f;
        doorRB.angularDrag = 0.3f;
        doorRB.mass = 1;
    }
}