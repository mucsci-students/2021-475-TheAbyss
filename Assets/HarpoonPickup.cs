using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            if(!controller.hasHarpoon())
            {
                controller.setHarpoonStatus(true);
                Destroy(this.gameObject);
            }
        }
    }
}
