using UnityEngine;

public class TrashCan : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Hat hat))
        {
            if (PlayerData.lastGrabObject == collision.gameObject) GrabbingController.Instance.UnGrabObject();

            hat.DeactivateHat();
            Destroy(collision.gameObject);
        }
    }
}
