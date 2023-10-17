using System.Collections.Generic;
using UnityEngine;

public class BathTubWater : MonoBehaviour
{
    [SerializeField] float cleanRate;

    List<Frog> frogs = new List<Frog>();
    Dictionary<Frog, int> frogCollidersInWater = new Dictionary<Frog, int>();

    void Update()
    {
        //foreach (Frog frog in frogs)
        //{
        //    frog.stats.hygieneStat.IncreaseStat(cleanRate * Time.deltaTime);
        //}

        foreach (KeyValuePair<Frog, int> frogPair in frogCollidersInWater)
        {
            frogPair.Key.stats.hygieneStat.IncreaseStat(cleanRate * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog) && !frogs.Contains(frog))
        {
            frog.stats.hygieneStat.DisableConsumption = true;
            //frogs.Add(frog);
            frogCollidersInWater.TryAdd(frog, 1); //Add frog if it's not registered
            frogCollidersInWater[frog]++; //Increment underwater collider count
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog))
        {
            frog.stats.hygieneStat.DisableConsumption = false;
            //frogs.Remove(frog);
            frogCollidersInWater[frog]--;
            if (frogCollidersInWater[frog] <= 0) frogCollidersInWater.Remove(frog);
        }
    }
}
