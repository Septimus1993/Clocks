namespace ClockEngine
{
    public interface ITime
    {
        void AddTime(double deltaTime, bool play);
        void SetTime(double totalTime, bool play);
    }

    public class ClockTimer : ITime
    {
        private readonly Clock clock;
        private readonly ClockInvoker invoker;

        public double time => this.clock.hoursHand.time;

        public ClockTimer(Clock clock, ClockInvoker invoker)
        {
            this.clock = clock;
            this.invoker = invoker;
        }

        public void Initialize()
        {
            this.invoker.Execute(hand => hand.Initialize(this));
        }

        public void AddTime(double deltaTime, bool play)
        {
            SetTime(this.time + deltaTime, play);
        }

        public void SetTime(double totalTime, bool play)
        {
            this.invoker.Execute(hand => hand.GoTo(totalTime, play));
        }
    }
}