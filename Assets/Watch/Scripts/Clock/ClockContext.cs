using UnityEngine;

namespace ClockEngine
{
    public class ClockContext : MonoBehaviour
    {
        [SerializeField]
        private HandData m_hours;

        [SerializeField]
        private HandData m_minutes;

        [SerializeField]
        private HandData m_seconds;

        [SerializeField]
        private EditMenuContext m_editMenu;

        private IEnable enabler { get; set; }
        public ITime time { get; private set; }

        private void OnEnable()
        {
            if (this.enabler == null)
                return;
            
            this.enabler.Enable();
        }

        private void OnDisable()
        {
            if (this.enabler == null)
                return;
            
            this.enabler.Disable();
        }

        public void Initialize()
        {
            var hourHand = this.m_hours.ToHand(
                totalTime => (float) (totalTime / 3600d % 24 * 3600d),
                3600d * 24d, 24d, 720d);

            var minuteHand = this.m_minutes.ToHand(
                totalTime => (float) (totalTime / 60d % 60 * 60d),
                3600d, 60d, 360d);

            var secondHand = this.m_seconds.ToHand(
                totalTime => (float) (totalTime % 60d),
                60d, 60d, 360d);

            var clock = new ClockFacade(hourHand, minuteHand, secondHand);
            this.enabler = clock;
            this.time = clock;

            clock.Initialize();
            this.m_editMenu.Initialize(clock);
        }
    }
}