using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject treasure;
    private Animation chestAnim;
    private PlayerController pc;
    private MessagesMap mm;
    // Start is called before the first frame update
    void Start()
    {
        chestAnim = GetComponentInChildren<Animation>();
        pc = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Player") && pc.hasKey)
        {
            chestAnim.Play();
            treasure.SetActive(true);
            pc.hasTreasure = true;
        }
    }
}
