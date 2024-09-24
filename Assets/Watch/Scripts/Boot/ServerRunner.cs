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
                throw new System.Exception(webRequest.error);

            var jsonResponse = webRequest.downloadHandler.text;
            var tempData = JsonConvert.DeserializeObject<JsonResponse?>(jsonResponse);

            if (!(tempData is JsonResponse data))
                throw new System.Exception("Failed  to parse JSON");

            LoadTime(data);
        }

        private IEnumerator LoadTimeLooped()
        {
            while (true)
            {
                StartCoroutine(LoadTime());

                yield return new WaitForSecondsRealtime(3600f);
            }
        }

        private void LoadTime(JsonResponse response)
        {
            if (this.context.isEditorMode)
                return;
            
            var totalTime = response.hour * 3600d + response.minute * 60d + response.seconds + response.milliseconds / 1000d;

            this.context.timer.SetTime(totalTime, true);
            this.context.enabled = true;
        }
    }

    public struct JsonResponse
    {
        public int hour;
        public int minute;
        public int seconds;
        public int milliseconds;
    }
}