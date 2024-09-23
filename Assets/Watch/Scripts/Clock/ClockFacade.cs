using System.Collections.Generic;

namespace ClockEngine
{
    public interface ITime
    {
        void AddTime(double deltaTime, bool play);
        void SetTime(double totalTime, bool play);
    }

    public class ClockFacade : ITime, IEdit, IEnable
    {
        private double lastTime;
        private double totalTime => this.hourHand.time;

        private readonly IClockHand hourHand;
        private readonly IClockHand minuteHand;
        private readonly IClockHand secondHand;

        public ClockFacade(IClockHand hourHand, IClockHand minuteHand, IClockHand secondHand)
        {
            this.hourHand = hourHand;
            this.minuteHand = minuteHand;
            this.secondHand = secondHand;
        }

        public void Initialize()
        {
            foreach (var hand in GetHands())
                hand.Initialize(this);

            SetNormalMode();
        }

        public void Enable()
        {
            foreach (var hand in GetHands())
                hand.Display();
        }

        public void Disable()
        {
            foreach (var hand in GetHands())
                hand.Hide();
        }

        public void SetEditMode()
        {
            this.lastTime = this.totalTime;

            foreach (var hand in GetHands())
                hand.SetEditMode();
        }

        public void SetNormalMode()
        {
            foreach (var hand in GetHands())
                hand.SetNormalMode();
        }

        public void ApplyChanges()
        {
            SetNormalMode();
        }

        public void RevertChanges()
        {
            SetTime(this.lastTime, false);
            SetNormalMode();
        }

        public void AddTime(double deltaTime, bool play)
        {
            SetTime(this.totalTime + deltaTime, play);
        }

        public void SetTime(double totalTime, bool play)
        {
            foreach (var hand in GetHands())
                hand.GoTo(totalTime, play);
        }

        private IEnumerable<IClockHand> GetHands()
        {
            yield return this.hourHand;
            yield return this.minuteHand;
            yield return this.secondHand;
        }
    }
}