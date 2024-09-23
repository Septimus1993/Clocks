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

        public event TimeLoadDelegate onLoad;

        public void Initialize()
        {
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

            this.onLoad?.Invoke(data.time, this.m_timeZoneId);
        }

        private IEnumerator LoadTimeLooped()
        {
            while (true)
            {
                StartCoroutine(LoadTime());

                yield return new WaitForSecondsRealtime(3600f);
            }
        }
    }

    public struct JsonResponse
    {
        public long time;
    }
}