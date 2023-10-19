using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float nutrition;
    Rigidbody rb;

    float spawnTime;

    [SerializeField] AudioClip eatFoodClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnTime = Time.time + 0.5f;
    }

    void Start()
    {
        PauseManager.Instance.RegisterRigidbody(rb);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"check time: {Time.time + 1f}")
        if (Time.time < spawnTime) return;

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

            GameManager.EatFood();
            SoundManager.Instance.PlayPooledSoundAtPosition(eatFoodClip, transform.position);
        }
    }
}
