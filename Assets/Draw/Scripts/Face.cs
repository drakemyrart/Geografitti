using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Face : MonoBehaviour
{
   
    private ARFaceManager arFaceMngr;
    
      
    [SerializeField]
    private GameObject clownNose;
    private bool nosed = false;

    // Start is called before the first frame update
    void Start()
    {
        
        arFaceMngr = FindObjectOfType<ARFaceManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        
    }

    private void UpdatePlacementPose()
    {
        ARFace face = arFaceMngr.GetComponentInChildren<ARFace>();
        if(face != null)
        {
            if (!nosed)
            {
                GameObject faceObj = face.gameObject;
                GameObject go = Instantiate(clownNose, faceObj.transform.position, Quaternion.identity) as GameObject;

                go.transform.SetParent(faceObj.transform);
                go.transform.localPosition = new Vector3(0f, 0f, -0.06f);

                nosed = true;
            }
            
        }
        else
        {
            nosed = false;
        }
    }
    

}
