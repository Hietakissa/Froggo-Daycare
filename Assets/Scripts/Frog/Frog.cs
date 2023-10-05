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
    [SerializeField] float angryThreshold = 50;
    [SerializeField] float furiousThreshold = 30;
    [SerializeField] float statCountThreshold = 3;

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

    [SerializeField] bool disableMovement;

    bool hasPath;
    float pathCalculationTime;
    float pathCalculationDelay = 1f;

    float cannotMoveTime;

    float waitUntilGettingPath;

    Vector3 GetRandomPosition => PathfindingManager.Instance.GetRandomPosition();

    [Header("Other")]
    [SerializeField] Transform hatHolder;
    GameObject equippedHat;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        roamingState.Init(this);

        currentState = roamingState;
        currentState.EnterState();
    }

    void Start()
    {
        CalculatePathToRandomPosition();
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

        if (disableMovement) return;

        waitUntilGettingPath -= Time.deltaTime;
        if (waitUntilGettingPath > 0f) return;

        if (cannotMoveTime > 5f)
        {
            rb.AddForce(Maf.Direction(transform.position, new Vector3(0f, 10f, 0f)) * 10, ForceMode.VelocityChange);
            cannotMoveTime = 0f;
        }
        else if (!hasPath && !isGrabbed && rb.velocity.magnitude < 1f)
        {
            pathCalculationTime += Time.deltaTime;

            if (pathCalculationTime >= pathCalculationDelay)
            {
                pathCalculationTime -= pathCalculationDelay;
                CalculatePathToRandomPosition();
            }
        }
    }


    public void EnterOven()
    {
        stats.consumptionMultiplier = 10f;
    }

    public void ExitOven()
    {
        stats.consumptionMultiplier = 1f;
    }

    void CalculatePath(Vector3 target)
    {
        Debug.Log($"Calculating path for {gameObject.name}");

        path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, target, 1, path))
        {
            if (GetPathLength(path) < 1f) CalculationFail();
            else CalculationSuccess();
        }
        else CalculationFail();

        void CalculationSuccess()
        {
            hasPath = true;

            pathCorners = path.corners;
            pathIndex = 0;
            nextPosition = pathCorners[pathIndex];

            cannotMoveTime = 0f;

            Debug.Log($"Path calculation succeeded for {gameObject.name}");
        }

        void CalculationFail()
        {
            Debug.Log($"Path calculation failed for {gameObject.name}");
            cannotMoveTime += pathCalculationDelay;

        }
    }

    void CompletePath()
    {
        Debug.Log("Path completed");
        hasPath = false;

        waitUntilGettingPath = 3f;
    }

    public void HandleMovement()
    {
        //if (pathCorners == null || pathIndex == pathCorners.Length) return;
        if (!hasPath || disableMovement) return;

        rb.position += Maf.Direction(transform.position, nextPosition) * speed * Time.deltaTime;
        //rb.MovePosition((transform.position + Maf.Direction(transform.position, nextPosition)).normalized * speed * Time.deltaTime);
        //Debug.DrawRay(transform.position, Maf.Direction(transform.position, nextPosition), Color.green, 5f);

        if ((Vector3.Distance(transform.position, nextPosition) < stoppingDistance))
        {
            pathIndex++;

            if (pathIndex < pathCorners.Length) nextPosition = pathCorners[pathIndex];
            else CompletePath();
        }
    }

    

    void SwitchState(FrogBaseState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }

    public void StartGrab()
    {
        isGrabbed = true;
        hasPath = false;
    }
    public void StopGrab()
    {
        isGrabbed = false;

        CalculatePathToRandomPosition();
    }

    void CalculatePathToRandomPosition()
    {
        CalculatePath(GetRandomPosition);
    }

    float GetPathLength(NavMeshPath path)
    {
        float length = 0f;

        for (int i = 0; i < path.corners.Length - 1; i++) length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        return length;
    }

    public void EquipHat(Transform hatObject)
    {
        hatHolder.DestroyChildren();

        hatObject.parent = hatHolder;
        hatObject.position = hatHolder.position;
        hatObject.rotation = hatHolder.rotation;
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
    public float consumptionMultiplier = 1f;

    public void ConsumeStats()
    {
        hungerStat.Consume(baseConsumption * consumptionMultiplier * Time.deltaTime);
        moodStat.Consume(baseConsumption * consumptionMultiplier * Time.deltaTime);
        energyStat.Consume(baseConsumption * consumptionMultiplier * Time.deltaTime);
        hygieneStat.Consume(baseConsumption * consumptionMultiplier * Time.deltaTime);
        toiletStat.Consume(baseConsumption * consumptionMultiplier * Time.deltaTime);
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