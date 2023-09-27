using UnityEngine.AI;
using UnityEngine;
using System;

public class Frog : MonoBehaviour, IGrabbable
{
    [Header("Stats")]
    [SerializeField] public Stat hungerStat;
    [SerializeField] public Stat moodStat;
    [SerializeField] public Stat energyStat;
    [SerializeField] public Stat hygieneStat;
    [SerializeField] public Stat toiletStat;

    [Header("Other")]
    [SerializeField] float baseConsumption;
    public string frogName;

    [Header("Mood Change")]
    [SerializeField] float angryThreshold;
    [SerializeField] float furiousThreshold;
    [SerializeField] float statAmount;

    //public event Action onStartGrab;
    //public event Action onStopGrab;

    public bool isGrabbed;

    FrogBaseState currentState;

    FrogBaseState roamingState = new FrogRoamingState();

    [SerializeField] Transform navigationTarget;

    void Awake()
    {
        roamingState.Init(this);

        currentState = roamingState;
        currentState.EnterState();
    }

    void Update()
    {
        NavMeshPath path = new NavMeshPath();

        if (NavMesh.CalculatePath(transform.position, navigationTarget.position, 1, path)) Debug.DrawRay(transform.position, Vector3.up * 15, Color.black);

        foreach (Vector3 corner in path.corners)
        {
            Debug.DrawRay(corner, Vector3.up * 2, Color.green);
        }

        for (int i = 0; i < path.corners.Length - 1; i++) Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);

        ConsumeStats();

        
    }


    void SwitchState(FrogBaseState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }



    void ConsumeStats()
    {
        hungerStat.Consume(baseConsumption * Time.deltaTime);
        moodStat.Consume(baseConsumption * Time.deltaTime);
        energyStat.Consume(baseConsumption * Time.deltaTime);
        hygieneStat.Consume(baseConsumption * Time.deltaTime);
        toiletStat.Consume(baseConsumption * Time.deltaTime);
    }
    int GetStatsUnderThreshold(float threshold)
    {
        int count = 0;

        if (hungerStat.IsAtOrUnderThreshold(threshold)) count++;
        if (moodStat.IsAtOrUnderThreshold(threshold)) count++;
        if (energyStat.IsAtOrUnderThreshold(threshold)) count++;
        if (hygieneStat.IsAtOrUnderThreshold(threshold)) count++;
        if (toiletStat.IsAtOrUnderThreshold(threshold)) count++;

        return count;
    }




    public void StartGrab()
    {
        //onStartGrab?.Invoke();
        GrabbingController.Instance.GrabObject();

        isGrabbed = true;
    }
    public void StopGrab()
    {
        //onStopGrab?.Invoke();
        GrabbingController.Instance.UnGrabObject();

        isGrabbed = false;
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