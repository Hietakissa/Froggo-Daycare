using UnityEngine;

public class FrogMouth : MonoBehaviour
{
    SkinnedMeshRenderer skin;

    [SerializeField] float speed;

    void Awake()
    {
        skin = GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        skin.SetBlendShapeWeight(0, Mathf.PingPong(Time.time * speed, 100));
    }
}
