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
        if (Physics.Raycast(PlayerData.cameraTransform.position, PlayerData.cameraTransform.forward, out RaycastHit hit, 5f, PlayerData.interactionMask))
        {
            //Debug.Log($"hit {hit.collider.name}");

            if (GameManager.TryGetFrog(hit.collider, out Frog frog))
            {
                frogStatString += frog.frogName;
                frogStatString += "\nHunger: " + frog.stats.hungerStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nMood: " + frog.stats.moodStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nEnergy: " + frog.stats.energyStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nHygiene: " + frog.stats.hygieneStat.GetStatValue().RoundToDecimalPlaces(2);
                frogStatString += "\nToilet: " + frog.stats.toiletStat.GetStatValue().RoundToDecimalPlaces(2);
            }
            

            text.text = frogStatString;
            frogStatString = string.Empty;
        }
        else text.text = "";
    }
}
