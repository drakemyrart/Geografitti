using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSGrabber : MonoBehaviour
{
    private float inLatitude;
    private float inLongitude;
    private float inTimeStamp;
    private bool gpslocation = false;

    public IEnumerator GrabbGPS()
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

            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public float Latitude
    {
        get
        {
            return inLatitude;
        }
    }

    public float Longitude
    {
        get
        {
            return inLongitude;
        }
    }

    public float TimeStamp
    {
        get
        {
            return inTimeStamp;
        }
    }

    public bool GPSFound
    {
        get
        {
            return gpslocation;
        }
    }

    public void ResetGPS()
    {
        gpslocation = false;
    }
}
