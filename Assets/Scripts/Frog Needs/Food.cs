using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float nutrition;
    Rigidbody rb;

    float spawnTime;

    [SerializeField] AudioClip eatFoodClip;

    bool eaten;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnTime = Time.timeSinceLevelLoad;
    }

    void Start()
    {
        PauseManager.Instance.RegisterRigidbody(rb);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"check time: {Time.time + 1f}")
        if (Time.timeSinceLevelLoad - spawnTime > 1f || eaten) return;

        if (GameManager.TryGetFrog(collision.collider, out Frog frog))
        {
            frog.stats.hungerStat.IncreaseStat(nutrition * Random.Range(0.9f, 1.2f));
            if (PlayerData.lastGrabObject == gameObject)
            {
                Debug.Log("Food ungrabbed object");
                GrabbingController.Instance.UnGrabObject();
            }
            PauseManager.Instance.UnregisterRigidbody(rb);
            Destroy(gameObject);

            eaten = true;
            GameManager.EatFood();
            SoundManager.Instance.PlayPooledSoundAtPosition(eatFoodClip, transform.position);
        }
    }
}
