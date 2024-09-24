namespace ClockEngine
{
    public interface IEditor : IEnable
    {
        void ApplyChanges();
        void RevertChanges();
    }

    public interface IEditable
    {
        void SetNormalMode();
        void SetEditMode();
    }

    public class ClockEditor : ClockInvoker<IEditable>, IEditor
    {
        private readonly ITimer timer;

        private double lastTime;

        public bool enabled { get; private set; }

        public ClockEditor(ClockTimer timer) : base(timer.clock)
        {
            this.timer = timer;
        }

        public void Enable()
        {
            if (this.enabled)
                return;

            this.enabled = true;
            this.lastTime = this.timer.time;

            Execute(hand => hand.SetEditMode());
        }

        public void Disable()
        {
            if (!this.enabled)
                return;

            this.enabled = false;

            Execute(hand => hand.SetNormalMode());
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