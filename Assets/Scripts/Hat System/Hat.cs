using UnityEngine;

public class Hat : MonoBehaviour, IGrabbable
{
    public HatSO hatSO;

    Collider hatCollider;
    Rigidbody rb;

    bool isGrabbed;

    Vector3 startPos;
    Quaternion startRot;

    float lastEquipped;

    void Awake()
    {
        hatCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;
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
            if (Time.time - lastEquipped < 2f) return;

            if (isGrabbed && PlayerData.lastGrabObject == gameObject)
            {
                Debug.Log("Hat ungrabbed object");
                GrabbingController.Instance.UnGrabObject();
            }
            
            //Destroy(hatCollider);
            //Destroy(rb);
            //Destroy(this);
            DeactivateHat();
            frog.EquipHat(transform, this, hatSO);
        }
    }

    public void ActivateHat()
    {
        transform.parent = null;
        hatCollider.enabled = true;
        rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(Random.insideUnitSphere * 3f, ForceMode.Impulse);

        lastEquipped = Time.time;
    }

    public void DeactivateHat()
    {
        if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();

        hatCollider.enabled = false;
        Destroy(rb);
    }
}
