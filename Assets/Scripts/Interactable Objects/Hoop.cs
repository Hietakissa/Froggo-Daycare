using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog) && frog.rb.velocity.y < -0.2f)
        {
            frog.stats.moodStat.IncreaseStat(30f);
            particles.Play();
        }
        else if (other.TryGetComponent(out Toy toy) && toy.rb.velocity.y < -0.2f) particles.Play();
    }
}
