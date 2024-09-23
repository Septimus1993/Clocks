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

        public ClockFacade clock;

        private void OnEnable()
        {
            if (this.clock == null)
                return;
            
            this.clock.Enable();
        }

        private void OnDisable()
        {
            if (this.clock == null)
                return;
            
            this.clock.Disable();
        }

        public void Initialize()
        {
            var hourArrowContext = this.m_hours.ToHand(
                totalTime => (float) (totalTime / 3600d % 24 * 3600d),
                3600d * 24d, 24d, 720d);

            var minuteArrowContext = this.m_minutes.ToHand(
                totalTime => (float) (totalTime / 60d % 60 * 60d),
                3600d, 60d, 360d);

            var secondArrowContext = this.m_seconds.ToHand(
                totalTime => (float) (totalTime % 60d),
                60d, 60d, 360d);

            var subContexts = new ClockHand[]
            {
                hourArrowContext,
                minuteArrowContext,
                secondArrowContext
            };

            this.clock = new ClockFacade(subContexts);

            this.clock.Initialize();
            this.m_editMenu.Initialize(this.clock);
        }
    }
}