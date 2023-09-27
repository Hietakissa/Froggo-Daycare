using HietakissaUtils;
using UnityEngine;
using TMPro;

public class FrogDebugOverlay : MonoBehaviour
{
    TextMeshProUGUI text;

    string frogStatString;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Physics.Raycast(PlayerData.cameraTransform.position, PlayerData.cameraTransform.forward, out RaycastHit hit, 5f))
        {
            //Debug.Log($"hit {hit.collider.name}");

            if (hit.collider.TryGetComponent(out Frog frog))
            {
                frogStatString += frog.frogName;
                frogStatString += "\nHunger: " + frog.hungerStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nMood: " + frog.moodStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nEnergy: " + frog.energyStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nHygiene: " + frog.hygieneStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nToilet: " + frog.toiletStat.GetStatValue().RoundToDecimalPlaces(2);
            }
            

            text.text = frogStatString;
            frogStatString = string.Empty;
        }
        else text.text = "no frog data found";
    }
}
