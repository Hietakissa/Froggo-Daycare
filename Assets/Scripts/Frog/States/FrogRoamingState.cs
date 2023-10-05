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
        frog.HandleMovement();
    }
}
