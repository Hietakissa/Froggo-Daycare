using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] AudioClip destroyHatClip;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Hat hat))
        {
            if (PlayerData.lastGrabObject == collision.gameObject) GrabbingController.Instance.UnGrabObject();

            hat.DeactivateHat();
            Destroy(collision.gameObject);
            SoundManager.Instance.PlayPooledSoundAtPosition(destroyHatClip, transform.position);
        }
    }
}
