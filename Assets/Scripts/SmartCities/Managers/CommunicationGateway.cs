using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SmartCities.API.Gira.Data;
using UnityEngine;

namespace SmartCities.Managers
{
    [RequireComponent(typeof(NetworkManager))]
    public class CommunicationGateway : MonoBehaviour
    {
        private NetworkManager networkManager;
        private string _basePath = "https://emel.city-platform.com/opendata";

        private void Awake()
        {
            networkManager = GetComponent<NetworkManager>();
        }

        public async UniTask<GiraStations> GetGiraStationList()
        {
            const string @params = @"/gira/station/list";

            var url = _basePath + @params;

            var result = await networkManager.GetRequest(url);
     
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            GiraStations giraStations = JsonConvert.DeserializeObject<GiraStations>(result);

            return giraStations;
        }
        //
        // public async UniTask<LadsContainer> GetLads(string queryParams=null)
        // {
        //     var @params = @"/lads";
        //     if (queryParams!=null)
        //     {
        //         @params += $"?page={queryParams}";
        //     }
        //
        //     var url = _basePath + @params;
        //
        //     var result = await networkManager.GetRequest(url);
        //
        //     if (string.IsNullOrEmpty(result))
        //     {
        //         return null;
        //     }
        //     
        //     var lads = JsonConvert.DeserializeObject<LadsContainer>(result);
        //
        //     return lads;
        // }
    }
}