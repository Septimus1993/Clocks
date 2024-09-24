namespace ClockEngine
{
    public class ClockDisplay
    {
        private readonly ClockInvoker clockInvoker;

        public ClockDisplay(ClockInvoker clockInvoker)
        {
            this.clockInvoker = clockInvoker;
        }

        public void Display()
        {
            this.clockInvoker.Execute(hand => hand.Display());
        }

        public void Hide()
        {
            this.clockInvoker.Execute(hand => hand.Hide());
        }
    }
}