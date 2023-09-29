using UnityEngine;

public class Oven : Appliance
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Frog frog))
        {
            frog.EnterOven();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Frog frog))
        {
            frog.ExitOven();
        }
    }
}
