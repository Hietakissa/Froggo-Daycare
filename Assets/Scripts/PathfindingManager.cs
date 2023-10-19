using System.Collections.Generic;
using HietakissaUtils;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager Instance;

    [SerializeField] Transform[] roamPoints;
    [SerializeField] float roamRadius;

    Dictionary<Frog, Transform> frogRoamAreas = new Dictionary<Frog, Transform>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterFrog(Frog frog)
    {
        frogRoamAreas.Add(frog, roamPoints.RandomElement());
    }

    public Vector3 GetRandomPosition(Frog frog)
    {
        //Vector3 randomPoint = roamPoints.RandomElement().position;
        if (Maf.RandomBool(35)) frogRoamAreas[frog] = roamPoints.RandomElement();

        Vector3 randomPoint = frogRoamAreas[frog].position;

        Vector3 randomOffset = Random.insideUnitCircle * roamRadius;
        randomOffset.y = 0;
        randomOffset = randomOffset.normalized;

        Debug.DrawRay(randomPoint + randomOffset, Vector3.up, Color.black, 3f);
        return randomPoint + randomOffset;
    }

#if UNITY_EDITOR
    [SerializeField] bool showGizmos;
    void OnDrawGizmos()
    {
        if (!showGizmos || roamPoints == null || roamPoints.Length == 0) return;

        foreach (Transform point in roamPoints) Gizmos.DrawWireSphere(point.position, roamRadius);
    }
#endif
}
