﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stroke : MonoBehaviour
{
    private Transform penPoint;

    public Color strokeColor;

    // Start is called before the first frame update
    void Start()
    {
        penPoint = GameObject.FindObjectOfType<Draw>().penPoint.transform;
    }

    // Update is called once per frame
    void Update()
    {
        penPoint = GameObject.FindObjectOfType<Draw>().penPoint.transform;
        

        if (Draw.drawing)
        {
            this.transform.position = penPoint.transform.position;
            this.transform.rotation = penPoint.transform.rotation;
        }
        else
        {
            this.enabled = false;
        }

    }


}
