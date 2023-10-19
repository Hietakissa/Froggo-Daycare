using UnityEngine;

public class FrogPottyState : FrogBaseState
{
    public override void EnterState()
    {
        frog.ShouldOverridePosition = true;

        frog.DisablePhysics();

        frog.stats.toiletStat.DisableConsumption = true;
    }

    public override void ExitState()
    {
        frog.ShouldOverridePosition = false;

        frog.EnablePhysics();

        frog.stats.toiletStat.DisableConsumption = false;
    }

    public override void UpdateState()
    {
        frog.stats.toiletStat.IncreaseStat(12f * Time.deltaTime);
    }

    public override void FixedUpdateState()
    {
        
    }
}
