using HietakissaUtils;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager Instance;

    [SerializeField] Transform[] roamPoints;
    [SerializeField] float roamRadius;

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 randomPoint = roamPoints.RandomElement().position;

        Vector3 randomOffset = Random.insideUnitCircle * roamRadius;
        randomOffset.y = 0;
        randomOffset = randomOffset.normalized;

        Debug.DrawRay(randomPoint + randomOffset, Vector3.up, Color.black, 3f);
        return randomPoint + randomOffset;
    }

    void OnDrawGizmos()
    {
        if (roamPoints == null || roamPoints.Length == 0) return;

        foreach (Transform point in roamPoints) Gizmos.DrawWireSphere(point.position, roamRadius);
    }
}
