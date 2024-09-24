namespace ClockEngine
{
    public delegate float CalculateTimeFunction(double totalTime);
    public delegate void TimeAddCallback(double time);
    public delegate void TimeLoadCallback(long timestamp, string timeZoneID);
    public delegate void ClockHandCallback(IClockHand clockHand);
}