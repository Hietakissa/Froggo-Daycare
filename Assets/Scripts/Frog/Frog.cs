using HietakissaUtils;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Frog : MonoBehaviour, IGrabbable
{
    [Header("Stats")]
    [SerializeField] public StatController stats;

    public string frogName;

    public bool isGrabbed;

    FrogBaseState currentState;
    FrogBaseState roamingState = new FrogRoamingState();
    FrogBaseState pottyState = new FrogPottyState();
    FrogBaseState furiousState = new FrogFuriousState();

    [HideInInspector] public Rigidbody rb;

    NavMeshPath path;
    Vector3[] pathCorners;
    int pathIndex;
    Vector3 nextPosition;

    float stoppingDistance = 0.3f;

    [Header("Furious")]
    [SerializeField] float furiousThreshold = 30;
    [SerializeField] float statCountThreshold = 3;
    [SerializeField] public LayerMask FuriousBounceMask;
    [SerializeField] public float BounceSpeed;
    [SerializeField] public float BounceDistanceOffset;

    [Header("Pathfinding/Movement")]
    [SerializeField] float speed = 2f;
    [SerializeField] float rotationSmoothing;

    [SerializeField] public bool DisableMovement;
    [HideInInspector] public bool ShouldOverridePosition;
    [HideInInspector] public Transform OverridePosition;

    bool hasPath;
    float pathCalculationTime;
    float pathCalculationDelay = 1f;

    float cannotMoveTime;

    float waitUntilGettingPath;

    Vector3 GetRandomPosition => PathfindingManager.Instance.GetRandomPosition();

    public event Action OnGrab;
    public event Action OnUnGrab;

    [Header("Other")]
    [SerializeField] Transform hatHolder;
    [SerializeField] int toyPlayChance = 8;
    [SerializeField] float toyCheckRadius = 2.5f;
    [SerializeField] float uprightTorque;
    [SerializeField] float uprightTorqueDamping;

    public float TimeNotOverriddenPositionFor;

    float toyPlayTime;
    float accumulatedToyPlayChance;

    [HideInInspector] public FrogAnimator animator;
    FloaterController floaterController;

    void Awake()
    {
        floaterController = GetComponent<FloaterController>();
        animator = GetComponent<FrogAnimator>();
        rb = GetComponent<Rigidbody>();

        roamingState.Init(this);
        pottyState.Init(this);
        furiousState.Init(this);

        currentState = roamingState;
        currentState.EnterState();
    }

    void Start()
    {
        //animator.applyRootMotion = true;
        //armature.localPosition = new Vector3(0f, 0.38f, 0f);

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

        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.PlayAnimation(FrogAnimation.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            animator.PlayAnimation(FrogAnimation.Walk);
        }

        stats.ConsumeStats();
        currentState.UpdateState();

        if (ShouldOverridePosition)
        {
            rb.position = OverridePosition.position;
            rb.rotation = OverridePosition.rotation;

            TimeNotOverriddenPositionFor = 0f;

            return;
        }
        else TimeNotOverriddenPositionFor += Time.deltaTime;

        if (stats.GetStatsUnderThreshold(furiousThreshold) >= statCountThreshold)
        {
            if (!StateIs(FrogState.Furious)) EnterState(FrogState.Furious);
            return;
        }
        else EnterState(FrogState.Roaming);

        Torque();

        PlayWithToys();

        if (DisableMovement) return;

        waitUntilGettingPath -= Time.deltaTime;
        if (waitUntilGettingPath > 0f) return;

        if (cannotMoveTime > 5f)
        {
            if (floaterController.underwater && stats.hygieneStat.GetStatValue() != 100f) return;

            rb.AddForce(Maf.Direction(transform.position, new Vector3(0f, 10f, 0f)) * 7, ForceMode.VelocityChange);
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

        void Torque()
        {
            Quaternion difference = Quaternion.FromToRotation(transform.up, Vector3.up);
            float angle;
            Vector3 axis;

            difference.ToAngleAxis(out angle, out axis);

            rb.AddTorque(-rb.angularVelocity * uprightTorqueDamping, ForceMode.Acceleration);
            rb.AddTorque(axis.normalized * angle * uprightTorque * Time.deltaTime, ForceMode.Acceleration);
        }

        void PlayWithToys()
        {
            toyPlayTime += Time.deltaTime;

            if (toyPlayTime >= 1f)
            {
                toyPlayTime -= 1f;

                accumulatedToyPlayChance += toyPlayChance;

                if (accumulatedToyPlayChance >= 100f)
                {
                    accumulatedToyPlayChance -= 100f;
                }
                else if (!Maf.RandomBool(toyPlayChance)) return;

                accumulatedToyPlayChance = 0f;

                Collider[] toys = Physics.OverlapSphere(transform.position, toyCheckRadius);

                foreach (Collider col in toys)
                {
                    if (col.CompareTag("Toy"))
                    {
                        col.GetComponent<Toy>().Fling();
                        stats.moodStat.IncreaseStat(40f);
                        animator.PlayAnimation(FrogAnimation.Play1);
                        return;
                    }
                }
            }
        }
    }


    public void EnterOven()
    {
        stats.consumptionMultiplier = 100f;
    }

    public void ExitOven()
    {
        stats.consumptionMultiplier = 1f;
    }

    void CalculatePath(Vector3 target)
    {
        //Debug.Log($"Calculating path for {gameObject.name}");

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

            //Debug.Log($"Path calculation succeeded for {gameObject.name}");
        }

        void CalculationFail()
        {
            //Debug.Log($"Path calculation failed for {gameObject.name}");
            cannotMoveTime += pathCalculationDelay;

        }
    }

    void CompletePath()
    {
        //Debug.Log("Path completed");
        hasPath = false;

        waitUntilGettingPath = UnityEngine.Random.Range(2.5f, 4f);
    }

    public void HandleMovement()
    {
        //if (pathCorners == null || pathIndex == pathCorners.Length) return;
        if (!hasPath || DisableMovement || rb.velocity.magnitude > 1f)
        {
            rb.freezeRotation = false;
            return;
        }
        else rb.freezeRotation = true;

        rb.position += Maf.Direction(transform.position, nextPosition) * speed * Time.deltaTime;
        //rb.MovePosition((transform.position + Maf.Direction(transform.position, nextPosition)).normalized * speed * Time.deltaTime);
        //Debug.DrawRay(transform.position, Maf.Direction(transform.position, nextPosition), Color.green, 5f);

        //lookRotation = Vector3.Lerp(lookRotation, Maf.Direction(transform.position, nextPosition), rotationSmoothing * Time.deltaTime);
        //transform.forward = transform.TransformDirection(lookRotation);

        Quaternion lookRotation = Quaternion.LookRotation(Maf.Direction(transform.position, nextPosition));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSmoothing * Time.deltaTime);

        if ((Vector3.Distance(transform.position, nextPosition) < stoppingDistance))
        {
            pathIndex++;

            if (pathIndex < pathCorners.Length)
            {
                nextPosition = pathCorners[pathIndex];
            }
            else CompletePath();
        }
    }

    public void EnablePhysics()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void DisablePhysics()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }


    public void EnterState(FrogState state)
    {
        if (GetStateForEnum(state) == currentState) return;

        ChangeState(GetStateForEnum(state));
    }

    void ChangeState(FrogBaseState nextState)
    {
        Debug.Log($"Changing state from {GetEnumForState(currentState)} to {GetEnumForState(nextState)}");

        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }

    public bool StateIs(FrogState state)
    {
        return currentState == GetStateForEnum(state);
    }

    FrogBaseState GetStateForEnum(FrogState enumState)
    {
        switch (enumState)
        {
            case FrogState.Roaming:
                return roamingState;
            case FrogState.Potty:
                return pottyState;
            case FrogState.Furious:
                return furiousState;
            default: return null;
        }
    }
    FrogState GetEnumForState(FrogBaseState state)
    {
        if (state.GetType() == typeof(FrogRoamingState)) return FrogState.Roaming;
        else if (state.GetType() == typeof(FrogPottyState)) return FrogState.Potty;
        else if (state.GetType() == typeof(FrogFuriousState)) return FrogState.Furious;
        Debug.Log($"Could not get enum for state type {state.GetType()}, {state}");
        return FrogState.Roaming;
    }

    public void StartGrab()
    {
        //if (ShouldOverridePosition) rb.isKinematic = false;

        Debug.Log("Grabbed frog");

        isGrabbed = true;
        hasPath = false;

        ShouldOverridePosition = false;

        EnterState(FrogState.Roaming);
        
        OnGrab?.Invoke();
    }
    public void StopGrab()
    {
        Debug.Log("Ungrabbed frog");

        cannotMoveTime = 0f;

        isGrabbed = false;

        CalculatePathToRandomPosition();

        OnUnGrab?.Invoke();
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

    public void EquipHat(Transform hatObject, HatSO hat)
    {
        hatHolder.DestroyChildren();

        hatObject.parent = hatHolder;
        hatObject.localPosition = hat.offset;
        hatObject.rotation = hatHolder.rotation;
    }
}

public enum FrogState
{
    Roaming,
    Potty,
    Furious
}

[System.Serializable]
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

[System.Serializable]
public class Stat
{
    [HideInInspector] public bool DisableConsumption;
    [SerializeField] float consumptionMultiplier = 1f;
    float value = 100f;

    float min = 0f, max = 100f;

    public void IncreaseStat(float amount)
    {
        amount = Mathf.Min(max - value, amount);

        LevelManager.Instance.AddXP(amount);

        value += amount;
        ClampStatValue();
    }
    public void DecreaseStat(float amount)
    {
        if (DisableConsumption) return;

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