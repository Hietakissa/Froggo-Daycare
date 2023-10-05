public abstract class FrogBaseState
{
    protected Frog frog;

    public void Init(Frog frog) => this.frog = frog;

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
}