using HietakissaUtils;
using UnityEngine;

public class FrogSoundController : MonoBehaviour
{
    [SerializeField] AudioClip[] impactSounds;
    [SerializeField] AudioClip changeHatSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip[] playSounds;
    [SerializeField] float impactThreshold;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();

        Frog frog = GetComponent<Frog>();
        frog.OnFrogJump += FrogJumped;
        frog.OnFrogChangeHat += FrogChangedHat;
        frog.OnFrogPlay += FrogPlayed;
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
