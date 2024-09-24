using System.Collections;
using System.Collections.Generic;

namespace ClockEngine
{
    public class Clock : IEnumerable<ClockHand>
    {
        public readonly ClockHand hoursHand;
        public readonly ClockHand minutesHand;
        public readonly ClockHand secondsHand;

        public Clock(ClockHand hoursHand, ClockHand minutesHand, ClockHand secondsHand)
        {
            this.hoursHand = hoursHand;
            this.minutesHand = minutesHand;
            this.secondsHand = secondsHand;
        }

        public IEnumerator<ClockHand> GetEnumerator()
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