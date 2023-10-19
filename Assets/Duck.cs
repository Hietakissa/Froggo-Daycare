using UnityEngine;

public class Duck : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip duckQuack;

    public void Interact()
    {
        SoundManager.Instance.PlayPooledSoundAtPosition(duckQuack, transform.position);
    }
}
