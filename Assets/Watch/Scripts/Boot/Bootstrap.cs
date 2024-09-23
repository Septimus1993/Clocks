using UnityEngine;

namespace ClockEngine
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private ClockContext m_clockContext;

        [SerializeField]
        private ServerRunner m_serverRunner;

        private void Start()
        {
            this.m_clockContext.Initialize();
            this.m_serverRunner.Initialize();

            this.m_serverRunner.onLoad += LoadTime;
        }

        private void LoadTime(long timeStamp, string timeZoneID)
        {
            var timeZone = System.TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
            var dateTime = System.DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).DateTime;
            var localTime = System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

            var totalTime = localTime.Hour * 3600d + localTime.Minute * 60d + localTime.Second + localTime.Millisecond / 1000d;

            this.m_clockContext.time.SetTime(totalTime, true);
            this.m_clockContext.enabled = true;
        }
    }
}
