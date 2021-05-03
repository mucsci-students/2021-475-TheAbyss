using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    private StatueShooter statueShooter;
    private AudioSource source;

    private void Start()
    {
        statueShooter = GetComponentInParent<StatueShooter>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Harpoon>())
        {
            statueShooter.TargetHit(this.gameObject);
            source.Play();
        }
    }
}
