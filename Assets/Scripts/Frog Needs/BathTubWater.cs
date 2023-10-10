using System.Collections.Generic;
using UnityEngine;

public class BathTubWater : MonoBehaviour
{
    [SerializeField] float cleanRate;

    List<Frog> frogs = new List<Frog>();

    void Update()
    {
        foreach (Frog frog in frogs)
        {
            frog.stats.hygieneStat.IncreaseStat(cleanRate * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Frog frog))
        {
            frog.stats.hygieneStat.DisableConsumption = true;
            frogs.Add(frog);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Frog frog))
        {
            frog.stats.hygieneStat.DisableConsumption = false;
            frogs.Remove(frog);
        }
    }
}
