using HietakissaUtils;
using System.Collections.Generic;
using UnityEngine;

public class BedArea : MonoBehaviour
{
    [SerializeField] float energyIncrease;

    List<Frog> frogs = new List<Frog>();

    void Update()
    {
        for (int i = frogs.Count - 1; i >= 0; i--)
        {
            Frog frog = frogs[i];

            frog.stats.energyStat.IncreaseStat(energyIncrease * Time.deltaTime);

            if (frog.stats.energyStat.GetStatValue() == 100f)
            {
                frog.stats.consumptionMultiplier = 1f;
                //frog.stats.energyStat.DisableConsumption = false;
                frog.disableMovement = false;

                frog.rb.AddForce(Maf.Direction(frog.rb.position, Vector3.up * 4f) * 8f, ForceMode.Impulse);

                frogs.Remove(frog);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
        if (other.TryGetComponent(out Frog frog))
        {
            Debug.Log("Frog enter");

            frog.stats.consumptionMultiplier = 0f;
            //frog.stats.energyStat.DisableConsumption = true;
            frog.disableMovement = true;
            frogs.Add(frog);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        if (other.TryGetComponent(out Frog frog))
        {
            Debug.Log("Frog exit");

            frog.stats.consumptionMultiplier = 1f;
            //frog.stats.energyStat.DisableConsumption = false;
            frog.disableMovement = false;
            frogs.Remove(frog);
        }
    }
}
