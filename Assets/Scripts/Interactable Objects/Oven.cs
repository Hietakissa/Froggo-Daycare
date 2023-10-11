using UnityEngine;

public class Oven : Appliance
{
    void OnTriggerEnter(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog))
        {
            frog.EnterOven();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog))
        {
            frog.ExitOven();
        }
    }
}
