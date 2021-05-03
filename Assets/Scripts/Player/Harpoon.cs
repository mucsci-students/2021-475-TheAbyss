using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{

    private float timer = 0;
    private float timeTillDestroy = 10;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeTillDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Enemy"))
        {
            print("Enemy hit");
            this.gameObject.GetComponent<TrailRenderer>().emitting = false;
            if(other.gameObject.GetComponent<SharkAI>() )
            {
                other.gameObject.GetComponent<SharkAI>().TakeDamageFromPlayer();
            }
            Destroy(this.gameObject);
        }
    }
}
