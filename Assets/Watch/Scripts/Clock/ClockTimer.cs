namespace ClockEngine
{
    public interface ITimer
    {
        double time { get; }

        void AddTime(double deltaTime, bool play);
        void SetTime(double totalTime, bool play);
    }

    public class ClockTimer : ClockInvoker<ITimerHand>, ITimer
    {
        public double time => this.clock.hoursHand.time;

        public ClockTimer(Clock clock) : base(clock) { }

        public void Initialize()
        {
            Execute(InitializeHand);
        }

        public void AddTime(double deltaTime, bool play)
        {
            SetTime(this.time + deltaTime, play);
        }

        public void SetTime(double totalTime, bool play)
        {
            void SetHandTime(ITimerHand hand)
            {
                hand.GoTo(totalTime, play);
            }

            Execute(SetHandTime);
        }

        private void InitializeHand(ITimerHand hand)
        {
            hand.Initialize(this);
        }
    }
}