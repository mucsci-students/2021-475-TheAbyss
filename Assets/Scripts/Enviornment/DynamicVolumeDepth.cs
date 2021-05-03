using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicVolumeDepth : MonoBehaviour
{

    public Transform player;
    public float maxDepth;

    public Light sun;

    private float currentPlayerDepth;
    private float depthProcessingFactor;

    private VolumeProfile processingVolume;
    private FilmGrain grain;
    private ColorAdjustments colorAdjustments;
    private float blueTint = 0.5f;
    private Vignette vignette;

    void Start()
    {
        processingVolume = GetComponent<Volume>().sharedProfile;
    }

    void Update()
    {
        currentPlayerDepth = Mathf.Abs(player.position.y);
        depthProcessingFactor = Mathf.Abs(currentPlayerDepth / maxDepth);
        UpdatePostProcessingVolume(depthProcessingFactor);
    }

    private void UpdatePostProcessingVolume(float depthFactor)
    {
        /*
        if(processingVolume.TryGet<FilmGrain>(out grain))
        {
            grain.intensity.Override(depthFactor);
        }
        */
        /*
        if(processingVolume.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.colorFilter.value = new Color(0.0f, 0.5f, Mathf.Clamp(blueTint/depthFactor, 0.0f, 1.0f), 1.0f);
        }
        */
        if(processingVolume.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.Override(Mathf.Lerp(0.1f, 0.25f, depthFactor));
        }
        
        sun.intensity = Mathf.Lerp(1.0f, 0.0f, depthFactor);
    }
}
