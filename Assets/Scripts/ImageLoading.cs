using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoading : MonoBehaviour
{
    [SerializeField]
    public Button btn;

    [SerializeField]
    public Canvas canvas;

    public int Imageid = 0;

    
    

    // Start is called before the first frame update
    void Start()
    {
        //btn.onClick.AddListener(LoadImage);
        //btn.interactable = false;
        canvas.worldCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadImage()
    {
        GraffitiPointOnMap.instance.RequestImage(Imageid);
    }

   
}
