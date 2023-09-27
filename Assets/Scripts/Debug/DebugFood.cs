using System.Collections;
using UnityEngine;

public class DebugFood : MonoBehaviour
{
    [SerializeField] float hungerReplenishAmount;

    SphereCollider collider;
    Rigidbody rb;

    void Awake()
    {
        collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Frog frog))
        {
            frog.hungerStat.IncreaseStat(hungerReplenishAmount);
            StartCoroutine(EatFoodCoroutine(transform.position, collision.transform));
        }
    }

    IEnumerator EatFoodCoroutine(Vector3 startPos, Transform target)
    {
        collider.enabled = false;
        rb.isKinematic = true;

        float t = 0f;

        while (transform.position != target.position)
        {
            t += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, target.position, t);
            yield return null;
        }

        if (PlayerData.lastGrabObject == gameObject) GrabbingController.Instance.UnGrabObject();
        Destroy(gameObject);
    }
}
