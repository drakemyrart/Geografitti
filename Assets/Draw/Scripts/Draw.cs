using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Draw : MonoBehaviour
{
    private ARRaycastManager arRaycast;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool paintPoseIsValid = false;

    public static bool drawing;
    
    public GameObject penPoint;

    [SerializeField]
    private GameObject paint;

    void Start()
    {
        arRaycast = FindObjectOfType<ARRaycastManager>();
        penPoint.SetActive(false);
        
    }

    void Update()
    {
        UpdatePlacementPose();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            Paint();
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended || !placementPoseIsValid)
        {
            EndPaint();
        }

        if (drawing)
        {
            penPoint.SetActive(true);
            
        }
        else
        {
            penPoint.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycast.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
        {
            penPoint.transform.position = hits[0].pose.position;
            penPoint.transform.rotation = hits[0].pose.rotation;

        }
    }

    private void Paint()
    {
        GameObject currentStroke;
        drawing = true;
        currentStroke = Instantiate(paint, penPoint.transform.position, penPoint.transform.rotation) as GameObject;
            
    }


    private void EndPaint()
    {
        drawing = false;
    }

}

