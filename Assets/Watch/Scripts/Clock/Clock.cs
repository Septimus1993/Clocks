using System.Collections;
using System.Collections.Generic;

namespace ClockEngine
{
    public class Clock : IEnumerable<IClockHand>
    {
        public readonly IClockHand hoursHand;
        public readonly IClockHand minutesHand;
        public readonly IClockHand secondsHand;

        public Clock(IClockHand hoursHand, IClockHand minutesHand, IClockHand secondsHand)
        {
            this.hoursHand = hoursHand;
            this.minutesHand = minutesHand;
            this.secondsHand = secondsHand;
        }

        public IEnumerator<IClockHand> GetEnumerator()
        {
            yield return this.hoursHand;
            yield return this.minutesHand;
            yield return this.secondsHand;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}