namespace ClockEngine
{
    public class ClockFacade
    {
        private ClockHand[] hands;

        private NormalClockMode normalMode;
        private EditClockMode editMode;
        private IClockMode currentMode;

        public ClockFacade(ClockHand[] hands)
        {
            this.hands = hands;

            this.normalMode = new NormalClockMode(this.hands);
            this.editMode = new EditClockMode(this.hands);
        }

        public void Initialize()
        {
            this.editMode.Initialize();
            SetNormalMode();
        }

        public void Enable()
        {
            foreach (var hand in this.hands)
                hand.Display();
        }

        public void Disable()
        {
            foreach (var hand in this.hands)
                hand.Hide();
        }

        public void SetEditMode()
        {
            SetMode(this.editMode);
        }

        public void SetNormalMode()
        {
            SetMode(this.normalMode);
        }

        private void SetMode(IClockMode mode)
        {
            this.currentMode = mode;
            this.currentMode?.Enable();
        }

        public void ApplyChanges()
        {
            SetNormalMode();
        }

        public void RevertChanges()
        {
            this.editMode.Revert();
            SetNormalMode();
        }

        public void SetTime(double totalTime, bool play)
        {
            this.currentMode.SetTime(totalTime, play);
        }
    }
}