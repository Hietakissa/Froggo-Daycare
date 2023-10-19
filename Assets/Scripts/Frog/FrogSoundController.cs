using HietakissaUtils;
using UnityEngine;

public class FrogSoundController : MonoBehaviour
{
    [SerializeField] AudioClip[] impactSounds;
    [SerializeField] AudioClip changeHatSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip[] playSounds;
    [SerializeField] AudioClip bathDoneSound;
    [SerializeField] float impactThreshold;

    AudioSource source;
    Frog frog;

    float lastHygiene;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        frog = GetComponent<Frog>();
        frog.OnFrogJump += FrogJumped;
        frog.OnFrogChangeHat += FrogChangedHat;
        frog.OnFrogPlay += FrogPlayed;
    }

    void Update()
    {
        if (frog.Underwater && frog.stats.hygieneStat.GetStatValue() == 100f && lastHygiene < 100f) SoundManager.Instance.PlayPooledSoundAtPosition(bathDoneSound, transform.position);
        lastHygiene = frog.stats.hygieneStat.GetStatValue();
    }

    void FrogJumped()
    {
        PlayAudioClip(jumpSound);
    }

    void FrogChangedHat()
    {
        PlayAudioClip(changeHatSound);
    }

    void FrogPlayed()
    {
        PlayAudioClip(playSounds.RandomElement());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude / Time.fixedDeltaTime >= impactThreshold)
        {
            //PlayAudioClip(impactSounds.RandomElement());
            SoundManager.Instance.PlayPooledSoundAtPosition(impactSounds.RandomElement(), transform.position);
        }
    }

    void PlayAudioClip(AudioClip clip)
    {
        source.clip = clip;
        source.pitch = SoundManager.Instance.GetPitch();
        source.Play();
    }
}
