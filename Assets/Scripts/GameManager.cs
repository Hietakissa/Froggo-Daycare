using System;
using UnityEngine;

public static class GameManager
{
    public static event Action OnPause;
    public static event Action OnUnPause;

    public static event Action OnEnterBook;
    public static event Action OnExitBook;

    public static event Action OnFoodEaten;

    public static event Action OnFuriousChange;

    public static int FuriousFrogCount;

    public static void Pause()
    {
        OnPause?.Invoke();
    }

    public static void UnPause()
    {
        OnUnPause?.Invoke();
    }



    public static void EnterBook()
    {
        OnEnterBook?.Invoke();
    }

    public static void ExitBook()
    {
        OnExitBook?.Invoke();
    }



    public static void EatFood()
    {
        OnFoodEaten?.Invoke();
    }



    public static void FrogEnterFurious()
    {
        FuriousFrogCount++;

        OnFuriousChange?.Invoke();
    }

    public static void FrogExitFurious()
    {
        FuriousFrogCount--;

        OnFuriousChange?.Invoke();
    }

    public static bool TryGetFrog(Collider coll, out Frog frog)
    {
        if (coll.TryGetComponent(out GrabBranch branch))
        {
            frog = branch.RootObject.GetComponent<Frog>();
            return true;
        }
        else
        {
            frog = null;
            return false;
        }
    }
}
