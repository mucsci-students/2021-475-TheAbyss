using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSonar : MonoBehaviour
{
    private GameObject shark;
    public AudioSource sonar;

    private float beepRate;
    public float beepRateFastest = 1.5f;
    public float beepRateSlowest = 10f;
    
    public float distanceInterpolationStart = 100f;
    private float lastBeep;
    private float beepTimer;

    public SharkUIPulser iconPulse;

    void Start()
    {
        shark = GameObject.FindGameObjectWithTag("Enemy");
        lastBeep = Time.time;
        beepRate = beepRateSlowest;
    }

    void Update()
    {
        beepTimer += Time.deltaTime;
        if(beepTimer >= lastBeep + beepRate)
        {
            lastBeep = Time.time;
            sonar.PlayOneShot(sonar.clip, 1.0f);
            iconPulse.Pulse();
        }

        float distance = Vector3.Distance(shark.transform.position, transform.position);
        if(distance <= distanceInterpolationStart)
        {
            float ratio = distance/distanceInterpolationStart;
            float result = Mathf.Lerp(beepRateFastest, beepRateSlowest, ratio);
            beepRate = result;
        }
    }
}
