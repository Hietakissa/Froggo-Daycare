using UnityEngine;

public class Hat : MonoBehaviour, IGrabbable
{
    public HatSO hatSO;

    MeshCollider hatCollider;
    Rigidbody rb;

    bool isGrabbed;

    void Awake()
    {
        hatCollider = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();
    }

    public void StartGrab()
    {
        isGrabbed = true;
    }

    public void StopGrab()
    {
        isGrabbed = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (GameManager.TryGetFrog(collision.collider, out Frog frog))
        {
            if (isGrabbed && PlayerData.lastGrabObject == gameObject)
            {
                Debug.Log("Hat ungrabbed object");
                GrabbingController.Instance.UnGrabObject();
            }
            
            Destroy(hatCollider);
            Destroy(rb);
            Destroy(this);
            frog.EquipHat(transform, hatSO);
        }
    }
}
