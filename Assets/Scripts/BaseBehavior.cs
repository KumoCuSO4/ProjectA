
using Interface;

public abstract class BaseBehavior : IMyDisposable
{
    protected bool isDisposed = false;
    
    public virtual void Dispose()
    {
        isDisposed = true;
    }

    public bool IsDisposed()
    {
        return isDisposed;
    }
}