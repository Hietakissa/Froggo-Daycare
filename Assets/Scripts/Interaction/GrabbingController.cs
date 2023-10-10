using HietakissaUtils;
using UnityEngine;

public class GrabbingController : MonoBehaviour
{
    public static GrabbingController Instance;

    [SerializeField] float grabbingDistance;
    [SerializeField] float grabbingForce;

    GameObject grabOrigin;
    Rigidbody grabbedRB;

    Transform transformOverride;
    bool objectHasTransformOverride;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!PlayerData.grabbingObject) return; 
        
        if (CalculateDistance(CalculateTargetPosition()) > grabbingDistance * 1.3f) UnGrabObject();
        //DebugText.Instance.AddText($"Grabbing object: {PlayerData.grabbingObject}, Distance: {CalculateDistanceToTarget(CalculateTargetPosition())}");
    }

    void FixedUpdate()
    {
        if (!PlayerData.grabbingObject) return;

        Vector3 targetPosition = CalculateTargetPosition();
        //Vector3 dirToGrabPos = Maf.Direction(grabOrigin.transform.position, targetPosition);

        float distance = CalculateDistance(targetPosition);

        if (PlayerData.GrabIsDoor)
        {
            Vector3 dirToGrabPos = Maf.Direction(grabOrigin.transform.position, targetPosition);

            grabbedRB.AddForceAtPosition(dirToGrabPos * distance * 700f * Time.deltaTime, grabOrigin.transform.position);
        }
        else
        {
            Vector3 dirToGrabPos = Maf.Direction((objectHasTransformOverride ? transformOverride.position : grabbedRB.position), targetPosition);
            Vector3 targetVelocity = dirToGrabPos * distance * grabbingForce;

            grabbedRB.velocity = targetVelocity;
        }

        Quaternion difference = Quaternion.FromToRotation(grabbedRB.transform.up, Vector3.up);
        float angle;
        Vector3 axis;

        difference.ToAngleAxis(out angle, out axis);

        grabbedRB.AddTorque(-grabbedRB.angularVelocity * 0.3f, ForceMode.Acceleration);
        grabbedRB.AddTorque(axis.normalized * angle * 10f * Time.deltaTime, ForceMode.Acceleration);

        //Debug.Log($"Velocity: {targetVelocity}, Magnitude: {targetVelocity.magnitude} {(PlayerData.GrabIsDoor ? "door" : "")}");
        //grabbedRB.AddForceAtPosition(CalculateTotalForce(dirToGrabPos, distance) * Time.deltaTime, grabOrigin.transform.position);
    }

    public void GrabObject()
    {
        PlayerData.grabbingObject = true;

        grabbedRB = PlayerData.lastGrabObject.GetComponent<Rigidbody>();
        if (!(PlayerData.lastGrabObject.TryGetComponent(out Frog frog) && frog.StateIs(FrogState.Furious))) grabbedRB.useGravity = false;
        grabbedRB.interpolation = RigidbodyInterpolation.Interpolate;

        //grabbedRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
        grabbedRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        grabbedRB.drag = 4f;
        grabbedRB.angularDrag = 2f;

        transformOverride = grabbedRB.transform.Find("Grab Transform Override");

        if (transformOverride != null) objectHasTransformOverride = true;
        else objectHasTransformOverride = false;

        if (PlayerData.GrabIsDoor)
        {
            if (grabOrigin == null) grabOrigin = new GameObject();
            grabOrigin.transform.position = PlayerData.lastGrabPoint;
            grabOrigin.transform.parent = PlayerData.lastGrabObject.transform;

            grabbingDistance = Mathf.Max(1.5f, Vector3.Distance(grabOrigin.transform.position, PlayerData.cameraTransform.position));
        }
        else grabbingDistance = Mathf.Max(1.5f, Vector3.Distance(grabbedRB.position, PlayerData.cameraTransform.position));
    }

    public void UnGrabObject()
    {
        PlayerData.grabbingObject = false;

        grabbedRB.useGravity = true;
        //grabbedRB.interpolation = RigidbodyInterpolation.None;

        grabbedRB.collisionDetectionMode = CollisionDetectionMode.Discrete;

        grabbedRB.drag = 0f;
        grabbedRB.angularDrag = 0.05f;

        PlayerData.lastGrab.StopGrab();
    }

    Vector3 CalculateTargetPosition()
    {
        return PlayerData.cameraTransform.position + PlayerData.cameraTransform.forward * grabbingDistance;
    }
    float CalculateDistance(Vector3 targetPosition)
    {
        Vector3 startPos;

        if (PlayerData.GrabIsDoor) startPos = grabOrigin.transform.position;
        else if (objectHasTransformOverride) startPos = transformOverride.position;
        else startPos = grabbedRB.position;

        return Vector3.Distance(startPos, targetPosition);
    }
    /*Vector3 CalculateTotalForce(Vector3 dir, float distance)
    {
        Debug.Log($"Calclated force: {(dir * grabbingForce * Mathf.Clamp(grabbingForceCurve.Evaluate(distance / grabbingDistance), 0.1f, 1f)).magnitude}, Direction: {dir}, Distance: {distance}, Force: {grabbingForce}, Curve Multiplier: {Mathf.Clamp(grabbingForceCurve.Evaluate(distance / grabbingDistance), 0.1f, 1f)}");
        return dir * grabbingForce * grabbingForceCurve.Evaluate(distance / grabbingDistance);
    }*/
}
