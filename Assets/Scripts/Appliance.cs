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
    }
}

enum Orientation
{
    Horizontal,
    Vertical
}