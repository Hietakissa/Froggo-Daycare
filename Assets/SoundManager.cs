using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioMixerGroup masterMixer;
    [SerializeField] float pitchDeviation = 0.22f;

    [SerializeField] AudioClip genericImpact;

    GameObject[] soundObjects = new GameObject[10];
    AudioSource[] soundSources = new AudioSource[10];
    int audioObjectIndex = 0;

    void Awake()
    {
        Instance = this;

        for (int i = 0; i < soundObjects.Length; i++)
        {
            soundObjects[i] = new GameObject($"Sound Holder Object {i + 1}");
            soundSources[i] = soundObjects[i].AddComponent<AudioSource>();
            soundSources[i].outputAudioMixerGroup = masterMixer;
            soundSources[i].playOnAwake = false;
        }
    }

    public void PlayGenericImpact(Vector3 position)
    {
        //GameObject sound = Instantiate(genericImpactSoundPrefab, position, Quaternion.identity);
        //sound.GetComponent<AudioSource>().pitch = GetPitch();
        //Destroy(sound, 5);
        if (Time.time < 0.5f) return;

        PlayPooledSoundAtPosition(genericImpact, position);
    }

    public void PlayPooledSoundAtPosition(AudioClip clip, Vector3 position)
    {
        soundObjects[audioObjectIndex].transform.position = position;
        AudioSource source = soundSources[audioObjectIndex];
        source.clip = clip;
        source.pitch = GetPitch();
        source.Play();

        audioObjectIndex++;
        audioObjectIndex %= 10;
    }

    public float GetPitch()
    {
        return 1 + Random.Range(-pitchDeviation * 0.5f, pitchDeviation * 0.5f);
    }
}
