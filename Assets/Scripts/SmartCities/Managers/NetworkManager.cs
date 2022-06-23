using Cysharp.Threading.Tasks;
using SmartCities.API;
using SmartCities.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace SmartCities.Managers
{
    public class NetworkManager : MonoBehaviour
    {
        private string _apiKey = "48dbce431974cf5e1a9f92d12b6e7d82";

        public async UniTask<string> GetRequest(string url)
        {
//            Debug.Log("Request: " + url);

            string result;

            UnityWebRequest request = new UnityWebRequest(url, "GET")
                {downloadHandler = (DownloadHandler) new DownloadHandlerBuffer()};

            request.SetRequestHeader(Headers.Key.Accept, Headers.Value.Json);
            request.SetRequestHeader(Headers.Key.ApiKey, _apiKey);
            
            request.timeout = 30; //This is not working

            _ = request.SendWebRequest();

            // Debug.Log(" Waiting request to complete");
            while (!request.isDone)
            {
                await UniTask.Delay(330);
            }

            // Debug.Log("Request is done");
            // Debug.Log("isNetworkError" + request.isNetworkError);
            // Debug.Log("isHttpError" + request.isHttpError);
            // Debug.Log("request.downloadHandler.text" + request.downloadHandler.text);
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                var jsonString = request.downloadHandler.text;

                if (jsonString.IsValidJson<string>())
                {
                    Debug.Log("<color=red>Error: </color> " + request.downloadHandler.text);
                }
                else
                {
                    Debug.LogWarning("<color=red>Error: </color> " +
                              "Not a valid json string." +
                              " Request " + request.downloadHandler.text);
                }

                return null;
            }

            while (!request.downloadHandler.isDone)
            {
                Debug.Log(request.downloadProgress);
                await UniTask.Delay(330);
            }

            result = request.downloadHandler.text;

            Debug.Log("Response: " + result);

            return result;
        }
    }
}