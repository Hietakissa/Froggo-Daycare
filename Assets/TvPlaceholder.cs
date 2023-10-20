using UnityEngine;

public class TvPlaceholder : MonoBehaviour
{
    void Awake()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
