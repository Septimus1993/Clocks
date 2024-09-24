using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ClockEngine
{
    public class ServerRunner : MonoBehaviour
    {
        [SerializeField]
        private string m_url;

        [SerializeField]
        private string m_timeZoneId;

        private ClockContext context;

        public void Initialize(ClockContext context)
        {
            this.context = context;

            StartCoroutine(LoadTimeLooped());
        }

        private IEnumerator LoadTime()
        {
            using var webRequest = UnityWebRequest.Get(this.m_url);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
                yield break;
            }

            var jsonResponse = webRequest.downloadHandler.text;
            var tempData = JsonConvert.DeserializeObject<JsonResponse?>(jsonResponse);

            if (!(tempData is JsonResponse data))
                throw new System.Exception("Failed  to parse JSON");

            LoadTime(data.time, this.m_timeZoneId);
        }

        private IEnumerator LoadTimeLooped()
        {
            while (true)
            {
                StartCoroutine(LoadTime());

                yield return new WaitForSecondsRealtime(3600f);
            }
        }

        private void LoadTime(long timeStamp, string timeZoneID)
        {
            if (this.context.isEditorMode)
                return;
            
            var timeZone = System.TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
            var dateTime = System.DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).DateTime;
            var localTime = System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);

            var totalTime = localTime.Hour * 3600d + localTime.Minute * 60d + localTime.Second + localTime.Millisecond / 1000d;

            this.context.timer.SetTime(totalTime, true);
            this.context.enabled = true;
        }
    }

    public struct JsonResponse
    {
        public long time;
    }
}