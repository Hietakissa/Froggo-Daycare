using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    [SerializeField] float moodIncrease;

    List<Frog> frogs = new List<Frog>();

    void Update()
    {
        foreach (Frog frog in frogs)
        {
            frog.stats.moodStat.IncreaseStat(moodIncrease * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Frog frog))
        {
            frog.stats.moodStat.DisableConsumption = true;
            frogs.Add(frog);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Frog frog))
        {
            frog.stats.moodStat.DisableConsumption = false;
            frogs.Remove(frog);
        }
    }
}
