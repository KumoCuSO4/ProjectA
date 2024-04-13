using System;

namespace Utils
{
    public interface IMyDisposable : IDisposable
    {
        public bool IsDisposed();
    }
}