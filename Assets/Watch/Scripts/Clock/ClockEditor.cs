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

            Execute(SetEditMode);
        }

        public void Disable()
        {
            if (!this.enabled)
                return;

            this.enabled = false;

            Execute(SetNormalMode);
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

        private static void SetNormalMode(IEditable editable)
        {
            editable.SetNormalMode();
        }

        private static void SetEditMode(IEditable editable)
        {
            editable.SetEditMode();
        }
    }
}