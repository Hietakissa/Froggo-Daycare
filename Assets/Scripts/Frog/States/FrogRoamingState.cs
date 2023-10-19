public class FrogRoamingState : FrogBaseState
{
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        frog.rb.freezeRotation = false;
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        frog.HandleMovement();
    }
}
