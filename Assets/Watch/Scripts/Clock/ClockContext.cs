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

        private ClockDisplay display;
        public ITime time { get; private set; }

        private void OnEnable()
        {
            if (this.display == null)
                return;
            
            this.display.Display();
        }

        private void OnDisable()
        {
            if (this.display == null)
                return;
            
            this.display.Hide();
        }

        public void Initialize()
        {
            var hoursHand = this.m_hours.ToHand(
                totalTime => (float) (totalTime / 3600d % 24 * 3600d),
                3600d * 24d, 24d, 720d);

            var minutesHand = this.m_minutes.ToHand(
                totalTime => (float) (totalTime / 60d % 60 * 60d),
                3600d, 60d, 360d);

            var secondsHand = this.m_seconds.ToHand(
                totalTime => (float) (totalTime % 60d),
                60d, 60d, 360d);

            var clock = new Clock(hoursHand, minutesHand, secondsHand);
            var invoker = new ClockInvoker(clock);
            var timer = new ClockTimer(clock, invoker);
            var editor = new ClockEditor(timer, invoker);

            this.display = new ClockDisplay(invoker);
            this.time = timer;

            timer.Initialize();
            this.m_editMenu.Initialize(editor);
        }
    }
}