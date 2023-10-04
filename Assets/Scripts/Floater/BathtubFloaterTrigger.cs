using UnityEngine;

public class BathtubFloaterTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FloaterController controller)) controller.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out FloaterController controller)) controller.enabled = false;
    }
}
