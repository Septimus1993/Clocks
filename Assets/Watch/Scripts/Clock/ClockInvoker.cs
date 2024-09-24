using System.Collections;

namespace ClockEngine
{
    public abstract class ClockInvoker<T> : EnumerableInvoker<T>
    {
        public readonly Clock clock;

        protected ClockInvoker(Clock clock)
        {
            this.clock = clock;
        }

        protected override IEnumerable GetInstances()
        {
            foreach (var hand in this.clock)
                yield return hand;
        }
    }
}