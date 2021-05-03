using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAirSupply : MonoBehaviour
{
    public GameObject airSupply;
    public float spawnTime;
    public float cannisterAmount;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnAir", spawnTime, spawnTime); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerStats>().RefillAir(cannisterAmount);
            airSupply.SetActive(false);
        }
    }

    void SpawnAir(){
        airSupply.SetActive(true);
    }
}
