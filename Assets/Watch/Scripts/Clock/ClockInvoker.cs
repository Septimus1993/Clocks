namespace ClockEngine
{
    public class ClockInvoker
    {
        private readonly Clock clock;

        public ClockInvoker(Clock clock)
        {
            this.clock = clock;
        }

        public void Execute(ClockHandCallback callback)
        {
            if (callback == null)
                return;

            foreach (var hand in this.clock)
                callback.Invoke(hand);
        }
    }
}