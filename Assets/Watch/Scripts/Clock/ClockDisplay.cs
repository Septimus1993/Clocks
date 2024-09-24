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
            Execute(hand => hand.Display());
        }

        public void Hide()
        {
            Execute(hand => hand.Hide());
        }
    }
}