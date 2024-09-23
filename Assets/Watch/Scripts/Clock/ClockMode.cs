namespace ClockEngine
{
    public interface IClockMode
    {
        void Enable();
        void SetTime(double totalTime, bool play);
    }

    public abstract class BaseClockMode : IClockMode
    {
        protected readonly ClockHand[] hands;

        protected abstract bool isPlaying { get; }

        protected BaseClockMode(ClockHand[] hands)
        {
            this.hands = hands;
        }

        public void Enable()
        {
            EnableInternal();
            
            foreach (var subContext in this.hands)
                EnableHand(subContext);
        }

        protected virtual void EnableInternal() { }

        protected abstract void EnableHand(ClockHand hand);

        protected void SetTime(double totalTime)
        {
            foreach (var subFacade in this.hands)
                subFacade.GoTo(totalTime, this.isPlaying);
        }

        public void SetTime(double totalTime, bool play)
        {
            if (play != this.isPlaying)
                return;

            SetTime(totalTime);
        }
    }

    public class NormalClockMode : BaseClockMode
    {
        protected override bool isPlaying => true;

        public NormalClockMode(ClockHand[] hands) : base(hands) { }

        protected override void EnableHand(ClockHand hand)
        {
            hand.SetNormalMode();
        }
    }

    public class EditClockMode : BaseClockMode
    {
        private double lastTime;
        private double totalTime;

        protected override bool isPlaying => false;

        public EditClockMode(ClockHand[] hands) : base(hands) { }

        public void Initialize()
        {
            foreach (var subContext in this.hands)
                subContext.Initialize(AddTime);
        }

        protected override void EnableInternal()
        {
            this.lastTime = this.totalTime = this.hands[0].time;
        }

        protected override void EnableHand(ClockHand hand)
        {
            hand.SetEditMode();
        }

        public void Revert()
        {
            SetTime(this.lastTime);
        }

        public void AddTime(double deltaTime)
        {
            this.totalTime += deltaTime;
            SetTime(this.totalTime);
        }
    }
}