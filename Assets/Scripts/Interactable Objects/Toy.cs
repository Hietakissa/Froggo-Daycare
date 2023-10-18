using UnityEngine;

public class Toy : MonoBehaviour
{
    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fling()
    {
        if (PlayerData.lastGrabObject == gameObject)
        {
            Debug.Log("Toy ungrabbed object");
            GrabbingController.Instance.UnGrabObject();
        }

        float horizontalForce = 0.75f;
        float verticalForce = 1.6f;
        Vector3 randomForce = new Vector3(Random.Range(-horizontalForce, horizontalForce), verticalForce, Random.Range(-horizontalForce, horizontalForce));
        rb.AddForce(randomForce, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * 3f);
    }
}
