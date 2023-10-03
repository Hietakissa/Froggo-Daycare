using UnityEngine.Events;
using UnityEngine;

public class WorldSpaceButton : MonoBehaviour
{
    [SerializeField] UnityEvent onClick;

    public void Click()
    {
        onClick?.Invoke();
    }
}
