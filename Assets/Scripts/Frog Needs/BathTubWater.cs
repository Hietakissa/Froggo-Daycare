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
            if (frogCollidersInWater.TryAdd(frog, 0))
            {
                //Add frog if it's not registered
                frog.Underwater = true;
            }
            frogCollidersInWater[frog]++; //Increment underwater collider count

            Debug.Log($"Frog collider in water, total of {frogCollidersInWater[frog]}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog))
        {
            
            //frogs.Remove(frog);
            frogCollidersInWater[frog]--;
            Debug.Log($"Frog collider exit water, total of {frogCollidersInWater[frog]}");
            if (frogCollidersInWater[frog] <= 0)
            {
                frogCollidersInWater.Remove(frog);
                frog.stats.hygieneStat.DisableConsumption = false;
                frog.Underwater = false;
            }
        }
    }
}
