using UnityEngine;

public class Toy : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fling()
    {
        if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();

        float horizontalForce = 1.5f;
        float verticalForce = 3.5f;
        Vector3 randomForce = new Vector3(Random.Range(-horizontalForce, horizontalForce), verticalForce, Random.Range(-horizontalForce, horizontalForce));
        rb.AddForce(randomForce, ForceMode.Impulse);
        rb.AddTorque(Random.onUnitSphere * 3f);
    }
}
