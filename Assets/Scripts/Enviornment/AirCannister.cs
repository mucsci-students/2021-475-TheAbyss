using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCannister : MonoBehaviour
{
    public float cannisterAmount;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerStats>().RefillAir(cannisterAmount);
            Destroy(gameObject);
        }
    }
}
