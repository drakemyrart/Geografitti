using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenManager : MonoBehaviour
{

    GraphicRaycaster graphicRaycaster;

    [SerializeField]
    GameObject scene1;
    [SerializeField]
    GameObject scene2;
    [SerializeField]
    GameObject scene3;

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
                if (hits.gameObject == scene1)
                {
                    SceneManager.LoadScene(1);
                }
                if (hits.gameObject == scene2)
                {
                    SceneManager.LoadScene(2);
                }
                if (hits.gameObject == scene3)
                {
                    SceneManager.LoadScene(3);
                }
            }
        }

    }
}
