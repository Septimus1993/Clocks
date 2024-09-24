namespace ClockEngine
{
    public interface IDisplay
    {
        void Display();
        void Hide();
    }

    public class ClockDisplay : ClockInvoker<IDisplay>
    {
        public ClockDisplay(Clock clock) : base(clock) { }

        public void Display()
        {
            Execute(DisplayHand);
        }

        public void Hide()
        {
            Execute(HideHand);
        }

        private static void DisplayHand(IDisplay hand)
        {
            hand.Display();
        }

        private static void HideHand(IDisplay hand)
        {
            hand.Hide();
        }
    }
}