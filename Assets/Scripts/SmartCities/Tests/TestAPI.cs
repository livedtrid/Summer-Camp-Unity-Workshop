using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using SmartCities.API.Gira;
using SmartCities.API.Gira.Data;
using SmartCities.Managers;
using UnityEngine;

[RequireComponent(typeof(CommunicationGateway))]
public class TestAPI : MonoBehaviour
{
    [SerializeField] private float _localLatitude = 38.7684f;
    [SerializeField] private float _localLongitude = -9.094f;
    [SerializeField] private float scaleFactor = 10000;
    [SerializeField] private ClippingSphere clippingSphere;



    [SerializeField] private GameObject poiPrefab;
    private CommunicationGateway _communicationGateway;

    // Start is called before the first frame update
    void Start()
    {
        _communicationGateway = GetComponent<CommunicationGateway>();

        Vector2 localCoordinates = new Vector2(_localLatitude, -_localLongitude);
        var coiso = GPSEncoder.USCToGPS(new Vector3(4315694, 0, -1005808));
        Debug.Log("COISO " + coiso);
        GPSEncoder.SetLocalOrigin(coiso);
    }

    public void GetGiraStations()
    {
        GetGiraStationsAsync();
    }

    private async void GetGiraStationsAsync()
    {
        GiraStations giraStations;

        giraStations = await _communicationGateway.GetGiraStationList();

        if (giraStations == null)
        {
            Debug.Log("giraStations list is null");
            return;
        }

        Debug.Log($"Total Gira Stations {giraStations.TotalFeatures}");


        foreach (var giraStation in giraStations.Features)
        {
            Vector3 spawnLocation = Vector3.zero;
            string debugText = $"Type {giraStation.Type}\n "; // +
            //$"Geometry.Type {giraStation.Geometry.Type}\n";


            var coordinates = giraStation.Geometry.Coordinates;

            foreach (var coordinate in coordinates)
            {
                var lat = coordinate[0];
                var lon = coordinate[1];

                //debugText += $"Geometry.Coordinates Latitude: {lat} Longitude: {lon}\n";

                var ucs = GPSEncoder.GPSToUCS(lat, lon);

                spawnLocation = new Vector3(ucs.x / scaleFactor * -1, ucs.y / scaleFactor * -1, ucs.z / scaleFactor);
                //debugText += $"Unity coordinate: {ucs}\n";
            }

            debugText += //$"IdExpl {giraStation.Properties.IdExpl}\n" +
                         //$"IdPlaneamento {giraStation.Properties.IdPlaneamento}\n" +
                $"DesigComercial {giraStation.Properties.DesigComercial}\n";
            // $"TipoServicoNiveis {giraStation.Properties.TipoServicoNiveis}\n" +
            //$"NumBicicletas {giraStation.Properties.NumBicicletas}\n" +
            //$"NumDocas {giraStation.Properties.NumDocas}\n" +
            //$"Racio {giraStation.Properties.Racio}\n" +
            //$"Estado {giraStation.Properties.Estado}\n";
            //$"UpdateDate {giraStation.Properties.UpdateDate}\n";


            //bbox = min Longitude , min Latitude , max Longitude , max Latitude 
            var boundingBox = giraStation.Properties.Bbox;
            var left = boundingBox[0];
            var bottom = boundingBox[1];
            var right = boundingBox[2];
            var top = boundingBox[3];

            //debugText += $"Bounding box {left}, {bottom}, {right}, {top}\n";
            Debug.Log(debugText);

            var poiGameObject = GameObject.Instantiate(poiPrefab, spawnLocation, Quaternion.identity, transform);
            poiGameObject.name = giraStation.Properties.DesigComercial;
            var poi = poiGameObject.GetComponent<Poi>();
            poi.UpdateText(debugText);

            if (clippingSphere)
            {
                foreach (var renderer in poiGameObject.GetComponentsInChildren<Renderer>())
                {
                    clippingSphere.AddRenderer(renderer);
                }
            }
        }
    }
}