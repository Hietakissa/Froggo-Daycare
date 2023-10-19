using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject lossScreen;
    [SerializeField] GameObject pauseScreen;

    //ruma systeemi mutta toimii (oletettavasti)

    void Win()
    {
        winScreen.SetActive(true);
    }

    void Lose()
    {
        lossScreen.SetActive(true);
    }

    void Pause()
    {
        if (GameManager.ShowMenuOnPause) pauseScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UnPause()
    {
        pauseScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        GameManager.OnWin += Win;
        GameManager.OnLose += Lose;

        GameManager.OnPause += Pause;
        GameManager.OnUnPause += UnPause;
    }
    void OnDisable()
    {
        GameManager.OnWin -= Win;
        GameManager.OnLose -= Lose;

        GameManager.OnPause -= Pause;
        GameManager.OnUnPause -= UnPause;
    }
}
