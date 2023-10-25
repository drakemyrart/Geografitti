using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using Assets.Scripts.Networking;
using System;
using UnityEngine.UI;

public class GraffitiPointOnMap : MonoBehaviour
{
    public static GraffitiPointOnMap instance;


    [SerializeField]
    AbstractMap _map;

    //[SerializeField]
    //[Geocode]
    //List<string> _locationStrings;
    Dictionary<int, Vector2d> _locations;
    //List<string> _tempLocationsStrings;
    Dictionary<int, Vector2d> _locationsD;
    public static Dictionary<int, Vector2d> _tempLocationsD;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    GameObject _markerPrefab;

    [SerializeField]
    GameObject _player;

    [SerializeField]
    GameObject graffitiPic;

    [SerializeField]
    Button btn;

    [SerializeField]
    Sprite loading;

    [SerializeField]
    Text txt;

    int id;

    Dictionary<int, GameObject> _spawnedObjects;

    private float inLatitude;
    private float inLongitude;
    private float inTimeStamp;
    private bool gpslocation = false;

    float currCountdownValue;

    

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
        _locations = new Dictionary<int, Vector2d>();
        //_tempLocationsStrings = new List<string>();
        //_locationStrings = new List<string>();
        _spawnedObjects = new Dictionary<int, GameObject>();
        _locationsD = new Dictionary<int, Vector2d>();
        _tempLocationsD = new Dictionary<int, Vector2d>();

        graffitiPic.SetActive(false);

        btn.interactable = false;
        btn.gameObject.SetActive(false);
        btn.onClick.AddListener(TaskONClick);

        StartCoroutine(StartCountdown(2f));

        
    }

    void TaskONClick()
    {
        DataSender.SendImageRequest(id);
    }

    public void StartCheck(int count)
    {
        
        StartCoroutine(CheckPOIs(count));
    }

    //get pois
    public IEnumerator CheckPOIs(int count)
    {
        
        while (_tempLocationsD.Count != count)
        {
            yield return new WaitForEndOfFrame();
            
        }
        if (_locationsD.Count == 0)
        {
            if (_tempLocationsD.Count != 0)
            {
                foreach (var item in _tempLocationsD)
                {
                    _locationsD.Add(item.Key, item.Value);
                }
            }
        }
        else
        {
            if (_tempLocationsD.Count > 0)
            {
                foreach (var item in _locationsD)
                {
                    if (!_tempLocationsD.ContainsKey(item.Key))
                    {
                        _locationsD.Remove(item.Key);
                    }
                }
                foreach (var item in _tempLocationsD)
                {
                    if (!_locationsD.ContainsKey(item.Key))
                    {
                        _locationsD.Add(item.Key, item.Value);
                    }
                }
            }
        }
        /*
        if (_locationsD.Count > 0)
        {
            foreach (var item in _locationsD)
            {
                foreach (var sec in _tempLocationsD)
                {
                    if (item.Key != sec.Key)
                    {
                        Vector3 itemValue = _map.GeoToWorldPosition(item.Value, true);
                        Vector3 secValue = _map.GeoToWorldPosition(sec.Value, true);
                        float dist = Vector3.Distance(itemValue, secValue);

                        if (dist < 2)
                        {
                            itemValue = itemValue + (Vector3.forward * 2);

                            Vector2d temp = new Vector2d();
                            temp = _map.WorldToGeoPosition(itemValue);
                            _locationsD[item.Key] = temp;
                        }
                        
                    }
                }
            }
        }
        */
        _tempLocationsD.Clear();
        
        foreach (var item in _locationsD)
        {
            var locationString = item.Value;
            if (!_locations.ContainsKey(item.Key))
            {
                _locations.Add(item.Key, item.Value);
                var instance = Instantiate(_markerPrefab);
                if (_locations.TryGetValue(item.Key, out Vector2d loc))
                {
                    instance.transform.localPosition = _map.GeoToWorldPosition(loc, true);
                    instance.GetComponentInChildren<ImageLoading>().Imageid = item.Key;
                }
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                _spawnedObjects.Add(item.Key, instance);
            }
            
        }
        
        foreach (var item in _locations)
        {
            
            if (!_locationsD.ContainsKey(item.Key))
            {
                _locations.Remove(item.Key);
                if (_spawnedObjects.ContainsKey(item.Key))
                {
                    if(_spawnedObjects.TryGetValue(item.Key, out GameObject value))
                    {
                        Destroy(value);
                        _spawnedObjects.Remove(item.Key);
                    }
                }
            }

        }
        txt.text = _locationsD.Count.ToString();
        
    }

    void GetTempPOIs(int lat, int lon)
    {
        DataSender.SendPOISearch(lat, lon);
        
    }

    //Do a search for POI
    public IEnumerator StartCountdown(float countdownValue)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        StartCoroutine(GrabbGPS());
       
        StartCoroutine(StartCountdown(30f));
    }

    //get image for loading
    public void RequestImage(int id)
    {
        if (id > 0)
        {
            Loading();
        }

        DataSender.SendImageRequest(id);
    }

    //Load image
    public void Loading()
    {
        graffitiPic.SetActive(true);
        graffitiPic.GetComponent<Image>().sprite = loading;
    }

    public void LoadImage(int id, string image, string username)
    {
        byte[] buff = Convert.FromBase64String(image);
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
        tex.LoadImage(buff);
        
        Sprite graffiti = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        
        graffitiPic.SetActive(true);
        graffitiPic.GetComponent<Image>().sprite = graffiti;
    }

    //close image
    public void CloseImage()
    {
        graffitiPic.SetActive(false);
    }

    //Get gps location
    IEnumerator GrabbGPS()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            gpslocation = false;
            yield break;
        }


        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            gpslocation = false;
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            gpslocation = false;
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            gpslocation = true;
            inLatitude = Input.location.lastData.latitude;
            inLongitude = Input.location.lastData.longitude;
            inTimeStamp = (float)Input.location.lastData.timestamp;

            GetTempPOIs((int)Math.Floor(inLatitude), (int)Math.Floor(inLongitude));

            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    
    public void ResetGPS()
    {
        gpslocation = false;
    }

    private void Update()
    {

        if (_spawnedObjects.Count > 0)
        {
            foreach (var item in _spawnedObjects)
            {

                GameObject spawnedObject = new GameObject();
                Vector2d location = new Vector2d();
                if (_spawnedObjects.TryGetValue(item.Key, out GameObject go))
                {
                    spawnedObject = go;

                }
                if (_locations.TryGetValue(item.Key, out Vector2d loc))
                {
                    location = loc;
                }

                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                                                
            }
        }

    }

    
}
