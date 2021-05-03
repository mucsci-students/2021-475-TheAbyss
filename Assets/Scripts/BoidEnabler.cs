using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidEnabler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("FishBoids"))
        {
            other.gameObject.GetComponent<NVBoids>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("FishBoids"))
        {
            other.gameObject.GetComponent<NVBoids>().enabled = false;
        }
    }
}
