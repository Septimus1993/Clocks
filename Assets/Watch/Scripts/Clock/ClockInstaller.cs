using UnityEngine;

namespace ClockEngine
{
    public class ClockInstaller : MonoBehaviour
    {
        [SerializeField]
        private HandData m_hours;

        [SerializeField]
        private HandData m_minutes;

        [SerializeField]
        private HandData m_seconds;

        [SerializeField]
        private EditMenuContext m_editMenu;

        public ClockContext Install()
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
            var timer = new ClockTimer(clock);
            var editor = new ClockEditor(timer);

            timer.Initialize();
            this.m_editMenu.Initialize(editor);

            var context = this.gameObject.AddComponent<ClockContext>();
            context.Initialize(timer, editor);

            return context;
        }
    }
}