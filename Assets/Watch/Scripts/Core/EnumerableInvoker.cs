using System;
using System.Collections;

namespace ClockEngine
{
    public abstract class EnumerableInvoker<T>
    {
        protected void Execute(Action<T> callback)
        {
            if (callback == null)
                return;

            foreach (T instance in GetInstances())
                callback.Invoke(instance);
        }

        protected abstract IEnumerable GetInstances();
    }
}