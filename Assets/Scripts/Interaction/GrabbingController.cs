using HietakissaUtils;
using UnityEngine;

public class GrabbingController : MonoBehaviour
{
    public static GrabbingController Instance;

    [SerializeField] float grabbingDistance;
    [SerializeField] float grabbingForce;

    Rigidbody grabbedRB;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!PlayerData.grabbingObject) return; 
        
        DebugText.Instance.AddText($"Grabbing object: {PlayerData.grabbingObject}, Distance: {Vector3.Distance(PlayerData.cameraTransform.position, grabbedRB.position)}");
    }

    void FixedUpdate()
    {
        if (!PlayerData.grabbingObject) return;

        //grabbedRB.AddForce((PlayerData.playerTransform.position - grabbedRB.position).normalized * Vector3.Distance(PlayerData.playerTransform.position, grabbedRB.position) * grabbingForce * Time.deltaTime);
        Vector3 targetPosition = CalculateTargetPosition();
        Vector3 targetVelocity = Maf.Direction(grabbedRB.position, targetPosition) * Vector3.Distance(grabbedRB.position, targetPosition) * grabbingForce;

        //grabbedRB.AddForce(Maf.Direction(grabbedRB.position, targetPosition) * Vector3.Distance(grabbedRB.position, targetPosition) * grabbingForce * Time.deltaTime);
        grabbedRB.velocity = targetVelocity;
    }

    public void GrabObject()
    {
        grabbedRB = PlayerData.lastGrabObject.GetComponent<Rigidbody>();
        grabbedRB.useGravity = false;
        grabbedRB.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void UnGrabObject()
    {
        grabbedRB.useGravity = true;
        grabbedRB.interpolation = RigidbodyInterpolation.None;
    }

    Vector3 CalculateTargetPosition()
    {
        return PlayerData.cameraTransform.position + PlayerData.cameraTransform.forward * grabbingDistance;
    }
}
