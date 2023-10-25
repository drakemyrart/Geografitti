using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackManager : MonoBehaviour
{
    GraphicRaycaster graphicRaycaster;

    [SerializeField]
    GameObject back;
    

    // Start is called before the first frame update
    void Start()
    {
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }

    // Update is called once per frame
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
                //create land
                if (hits.gameObject == back)
                {
                    SceneManager.LoadScene(0);
                }
                
            }
        }

    }
}
