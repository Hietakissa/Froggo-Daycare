using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Quit Game!");
#else
        Application.Quit();
#endif
    }
}
