namespace ClockEngine
{
    public interface IEditor : IEnable
    {
        void ApplyChanges();
        void RevertChanges();
    }

    public class ClockEditor : IEditor
    {
        private readonly ClockTimer timer;
        private readonly ClockInvoker invoker;

        private double lastTime;
        private bool enabled;

        public ClockEditor(ClockTimer timer, ClockInvoker invoker)
        {
            this.timer = timer;
            this.invoker = invoker;
        }

        public void Enable()
        {
            if (this.enabled)
                return;

            this.enabled = true;
            this.lastTime = this.timer.time;
            this.invoker.Execute(hand => hand.SetEditMode());
        }

        public void Disable()
        {
            if (!this.enabled)
                return;

            this.enabled = false;
            this.invoker.Execute(hand => hand.SetNormalMode());
        }

        public void ApplyChanges()
        {
            Disable();
        }

        public void RevertChanges()
        {
            if (!this.enabled)
                return;

            this.timer.SetTime(this.lastTime, false);
            Disable();
        }
    }
}