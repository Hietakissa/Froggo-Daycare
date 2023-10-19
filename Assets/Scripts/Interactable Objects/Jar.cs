using UnityEngine;

public class Jar : MonoBehaviour, IInteractable, IGrabbable
{
    [SerializeField] Transform spawnPos;
    Rigidbody rb;

    [SerializeField] AudioClip[] shatterSounds;
    [SerializeField] AudioClip impactSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PauseManager.Instance.RegisterRigidbody(rb);
    }

    public void Interact()
    {
        Activate();
    }

    public void Activate(bool spawnAngry = false)
    {
        FrogSpawner.Instance.SpawnFrog(spawnPos, spawnAngry);
        LevelManager.Instance.AddXP(100);

        if (PlayerData.lastGrabObject == gameObject)
        {
            Debug.Log("Jar ungrabbed object");
            GrabbingController.Instance.UnGrabObject();
        }

        PauseManager.Instance.UnregisterRigidbody(rb);
        Destroy(gameObject);

        if (spawnAngry)
        {
            foreach (AudioClip shatterSound in shatterSounds) SoundManager.Instance.PlayPooledSoundAtPosition(shatterSound, transform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude / Time.fixedDeltaTime >= 0.3f)
        {
            SoundManager.Instance.PlayPooledSoundAtPosition(impactSound, transform.position);
        }
    }

    public void StartGrab()
    {
        
    }

    public void StopGrab()
    {
        
    }
}
