using UnityEngine;

namespace ClockEngine
{
    public class ClockContext : MonoBehaviour
    {
        private ClockDisplay display;
        private ClockEditor editor;

        public ITimer timer { get; private set; }
        public bool isEditorMode => this.editor.enabled;

        public void Initialize(ClockTimer timer, ClockEditor editor)
        {
            this.display = new ClockDisplay(timer.clock);
            this.editor = editor;
            this.timer = timer;
        }

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
    }
}