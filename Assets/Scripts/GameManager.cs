using System;

public static class GameManager
{
    public static event Action OnPause;
    public static event Action OnUnPause;

    public static void Pause()
    {
        OnPause?.Invoke();
    }

    public static void UnPause()
    {
        OnUnPause?.Invoke();
    }
}
