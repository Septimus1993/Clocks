using System.Collections.Generic;

namespace ClockEngine
{
    public abstract class ClockInvoker<T> : EnumerableInvoker<T> where T : class
    {
        public readonly Clock clock;

        protected ClockInvoker(Clock clock)
        {
            this.clock = clock;
        }

        protected override IEnumerable<T> GetInstances()
        {
            foreach (var hand in this.clock)
                yield return hand as T;
        }
    }
}