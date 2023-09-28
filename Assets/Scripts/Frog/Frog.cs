using HietakissaUtils;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Frog : MonoBehaviour, IGrabbable
{
    [Header("Stats")]
    [SerializeField] public StatController stats;

    public string frogName;

    [Header("Mood Change")]
    [SerializeField] float angryThreshold;
    [SerializeField] float furiousThreshold;
    [SerializeField] float statCountThreshold;

    public bool isGrabbed;

    FrogBaseState currentState;
    FrogBaseState roamingState = new FrogRoamingState();

    [SerializeField] Transform navigationTarget;
    Rigidbody rb;

    NavMeshPath path;
    Vector3[] pathCorners;
    int pathIndex;
    Vector3 nextPosition;

    float stoppingDistance = 0.3f;
    [SerializeField] float speed = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        roamingState.Init(this);

        currentState = roamingState;
        currentState.EnterState();

        CalculatePath(navigationTarget.position);
    }

    void Update()
    {
        //if (NavMesh.CalculatePath(transform.position, navigationTarget.position, 1, path)) Debug.DrawRay(transform.position, Vector3.up * 15, Color.black);

        /*foreach (Vector3 corner in path.corners)
        {
            Debug.DrawRay(corner, Vector3.up * 2, Color.green);
        }

        for (int i = 0; i < path.corners.Length - 1; i++) Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);*/

        stats.ConsumeStats();
        currentState.UpdateState();

        HandleMovement();
    }


    void CalculatePath(Vector3 target)
    {
        Debug.Log($"Calculating path for {gameObject.name}");

        path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, target, 1, path))
        {
            pathCorners = path.corners;
            pathIndex = 0;
            nextPosition = pathCorners[pathIndex];
            Debug.Log($"Path calculation succeeded for {gameObject.name}");
        }
        else Debug.Log($"Path calculation failed for {gameObject.name}");
    }

    void HandleMovement()
    {
        if (isGrabbed || pathCorners == null || pathIndex == pathCorners.Length) return;

        rb.position += Maf.Direction(transform.position, nextPosition) * speed * Time.deltaTime;

        if ((Vector3.Distance(transform.position, nextPosition) < stoppingDistance))
        {
            pathIndex++;

            if (pathIndex < pathCorners.Length) nextPosition = pathCorners[pathIndex];
            else CompletePath();
        }
    }

    void CompletePath()
    {
        Debug.Log("Path completed");
    }

    void SwitchState(FrogBaseState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }

    public void StartGrab()
    {
        GrabbingController.Instance.GrabObject();

        isGrabbed = true;

        Debug.Log("Grabbed");
    }
    public void StopGrab()
    {
        GrabbingController.Instance.UnGrabObject();

        isGrabbed = false;

        Debug.Log("Ungrabbed");
        CalculatePath(navigationTarget.position);
    }
}

[Serializable]
public class StatController
{
    [SerializeField] public Stat hungerStat;
    [SerializeField] public Stat moodStat;
    [SerializeField] public Stat energyStat;
    [SerializeField] public Stat hygieneStat;
    [SerializeField] public Stat toiletStat;

    [SerializeField] float baseConsumption = 2f;

    public void ConsumeStats()
    {
        hungerStat.Consume(baseConsumption * Time.deltaTime);
        moodStat.Consume(baseConsumption * Time.deltaTime);
        energyStat.Consume(baseConsumption * Time.deltaTime);
        hygieneStat.Consume(baseConsumption * Time.deltaTime);
        toiletStat.Consume(baseConsumption * Time.deltaTime);
    }
    public int GetStatsUnderThreshold(float threshold)
    {
        int count = 0;

        if (hungerStat.IsAtOrUnderThreshold(threshold)) count++;
        if (moodStat.IsAtOrUnderThreshold(threshold)) count++;
        if (energyStat.IsAtOrUnderThreshold(threshold)) count++;
        if (hygieneStat.IsAtOrUnderThreshold(threshold)) count++;
        if (toiletStat.IsAtOrUnderThreshold(threshold)) count++;

        return count;
    }
}

[Serializable]
public class Stat
{
    [SerializeField] float consumptionMultiplier = 1f;
    float value = 100f;

    float min = 0f, max = 100f;

    public void IncreaseStat(float amount)
    {
        value += amount;
        ClampStatValue();
    }
    public void DecreaseStat(float amount)
    {
        value -= amount;
        ClampStatValue();
    }
    public void Consume(float amount)
    {
        DecreaseStat(amount * consumptionMultiplier);
    }

    void ClampStatValue()
    {
        value = Mathf.Clamp(value, min, max);
    }
    public float GetStatValue() => value;

    public bool IsAtOrUnderThreshold(float threshold) => value <= threshold;
}