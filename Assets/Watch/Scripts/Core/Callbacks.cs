namespace ClockEngine
{
    public delegate float CalculateTimeFunction(double totalTime);
    public delegate void TimeAddCallback(double time);
    public delegate void TimeLoadDelegate(long timestamp, string timeZoneID);
}