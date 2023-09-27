public abstract class FrogBaseState
{
    public abstract void Init(Frog frog);

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
}