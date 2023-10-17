using HietakissaUtils;
using UnityEngine;

public class StatBubbleController : MonoBehaviour
{
    [SerializeField] Frog frog;
    [SerializeField] float minBubbleThreshold = 65f;

    [SerializeField] GameObject[] statBubbles;

    FrogStat lastStat = FrogStat.None;

    void Update()
    {
        float statValue = frog.stats.GetLowestStat(out FrogStat stat);

        Debug.Log($"Lowest stat '{stat}' with value of '{statValue}'");

        if (statValue < minBubbleThreshold)
        {
            DeactivateLastBubble();

            statBubbles[(int)stat].SetActive(true);
            lastStat = stat;

            OrientTowardsPlayer();
        }
        else DeactivateLastBubble();
    }

    void DeactivateLastBubble()
    {
        if (lastStat != FrogStat.None) statBubbles[(int)lastStat].SetActive(false);
    }

    void OrientTowardsPlayer()
    {
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Maf.Direction(transform.position, PlayerData.cameraHolder.position).SetY(0f)), 30f * Time.deltaTime);
        transform.rotation = rot;
    }
}
