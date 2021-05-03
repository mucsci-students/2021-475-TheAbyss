using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkVisionCone : MonoBehaviour
{
    public SharkAI shark;
    public GameObject player;

    private RaycastHit hit;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hit))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                shark.playerCanBeRaycasted = true;
            }
            else
            {
                shark.playerCanBeRaycasted = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            shark.playerIsInVisionCone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            shark.playerIsInVisionCone = false;
        }
    }
}
