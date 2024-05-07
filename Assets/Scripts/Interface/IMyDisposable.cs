using System;

namespace Interface
{
    public interface IMyDisposable : IDisposable
    {
        public bool IsDisposed();
    }
}