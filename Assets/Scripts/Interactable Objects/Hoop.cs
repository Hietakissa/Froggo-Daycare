using UnityEngine;

public class Hoop : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    float lastPlayTime;

    void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastPlayTime < 0.5f) return;

        if (GameManager.TryGetFrog(other, out Frog frog) && frog.rb.velocity.y < -0.2f)
        {
            frog.stats.moodStat.IncreaseStat(30f);
            particles.Play();
            lastPlayTime = Time.time;
        }
        else if (other.TryGetComponent(out Toy toy) && toy.rb.velocity.y < -0.2f)
        {
            particles.Play();
            lastPlayTime = Time.time;
        }
    }
}
