using System;
using System.Collections.Generic;

namespace ClockEngine
{
    public abstract class EnumerableInvoker<T>
    {
        protected void Execute(Action<T> callback)
        {
            if (callback == null)
                return;

            foreach (var instance in GetInstances())
                callback.Invoke(instance);
        }

        protected abstract IEnumerable<T> GetInstances();
    }
}