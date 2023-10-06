using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] float buoyancy = 50f;
    [HideInInspector] public LayerMask mask;

    Rigidbody rb;

    Vector3 startPos;

    RaycastHit hit;

    public void Init(FloaterController controller)
    {
        mask = controller.waterMask;
        rb = controller.rb;
    }

    public void Process()
    {
        rb.AddForceAtPosition(Vector3.up * buoyancy * GetDepth() * Time.deltaTime, transform.position, ForceMode.VelocityChange);
    }

    public bool IsUnderwater()
    {
        startPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        return Physics.Raycast(startPos, Vector3.down, out hit, 1f, mask);
    }

    float GetDepth()
    {
        return hit.point.y - transform.position.y;
    }

    void OnDrawGizmos()
    {
        if (mask.value == 0) mask = LayerMask.GetMask("Water");

        if (IsUnderwater())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector3.up * GetDepth());
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hit.point, Vector3.up * GetDepth());
        }
    }
}
