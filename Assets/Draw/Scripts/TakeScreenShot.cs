using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Networking;
using System.IO;
using UnityEngine.EventSystems;
using System;

public class TakeScreenShot : MonoBehaviour
{
    GraphicRaycaster graphicRaycaster;

    [SerializeField]
    GameObject back;
    [SerializeField]
    GameObject pic;
    [SerializeField]
    GameObject sh;
    [SerializeField]
    GameObject can;
    [SerializeField]
    GameObject interactor;

    private float inLatitude;
    private float inLongitude;
    private float inTimeStamp;
    private bool gpslocation = false;

    private void Start()
    {
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        sh.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Ui clicked");
            }
            //Check if a UI element is hit  
            PointerEventData ped = new PointerEventData(null);

            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            graphicRaycaster.Raycast(ped, results);
            foreach (RaycastResult hits in results)
            {
                
                if (hits.gameObject == back)
                {
                    GameManager.instance.Loading(1); 
                }
                if (hits.gameObject == pic)
                {
                    TakeShot();
                }
                
            }
        }

    }

    public void TakeShot()
    {
        can.SetActive(false);
        back.SetActive(false);
        pic.SetActive(false);
        StartCoroutine(Capture());
        
    }

    public IEnumerator Capture()
    {
        //screenshot
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForSeconds(1.5f);

        //show screenshot on screen
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
        tex.LoadImage(GetScreenshotImageBytes(pathToSave));
        

        Sprite graffiti = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        sh.GetComponent<Image>().sprite = graffiti;
        StartCoroutine(SendImageToServer(pathToSave));
        yield return new WaitForSeconds(4.0f);
        sh.SetActive(false);
       

    }
    //Get the screenshot from internal data
    byte[] GetScreenshotImageBytes(string filePath)
    {
        
        byte[] fileBytes = null;
        string item = Application.persistentDataPath + "/" + filePath;
         

        if (File.Exists(item))
        {
            sh.SetActive(true);
            fileBytes = File.ReadAllBytes(item);
            

        }


        return fileBytes;
    }
    //send image as base64 string
    IEnumerator SendImageToServer(string filePath)
    {
        StartCoroutine(GrabbGPS());
        while (!gpslocation)
        {
            yield return new WaitForEndOfFrame();
        }
        string pic = Convert.ToBase64String(GetScreenshotImageBytes(filePath));
        float lat = inLatitude;
        float lon = inLongitude;
        
        ResetGPS();
        DataSender.SendImage(pic, lat, lon);

        StartCoroutine(GraffitiPointOnMap.instance.StartCountdown(5f));
        GameManager.instance.Loading(1);
        
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

            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public void ResetGPS()
    {
        gpslocation = false;
    }
}
