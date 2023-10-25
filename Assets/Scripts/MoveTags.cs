using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTags : MonoBehaviour
{
    int imageid = 0;
    bool running = false;
    public bool obstructed = false;

    [SerializeField]
    ImageLoading imageLoading;

    private void Start()
    {
        imageid = imageLoading.Imageid;
    }

    private void OnTriggerEnter(Collider other)
    {
        obstructed = true;
        /*
        int temp = other.gameObject.GetComponentInChildren<ImageLoading>().Imageid;
        if (temp > imageid)
        {
            StartCoroutine(GetRoom());
        }
        */
    }

    private void OnTriggerStay(Collider other)
    {
        obstructed = true;
        /*
        int temp = other.gameObject.GetComponentInChildren<ImageLoading>().Imageid;
        if (temp > imageid)
        {
            if (!running)
            {
                StartCoroutine(GetRoom());
            }

        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        obstructed = false;
        /*
        int temp = other.gameObject.GetComponentInChildren<ImageLoading>().Imageid;
        if (temp > imageid)
        {
            StopCoroutine(GetRoom());
            running = false;
        }
        */
    }

    IEnumerator GetRoom()
    {
        running = true;
        while (true)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(1f, 0f, 0f);
        }
    }
}
