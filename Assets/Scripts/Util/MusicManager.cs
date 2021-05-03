using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicKick;

    private bool isSharkChasing;

    public void SetSharkChasing(bool c)
    {
        isSharkChasing = c;
    }

    void Update()
    {
        if(isSharkChasing)
        {
            musicKick.volume = 1.0f;
        }
        else
        {
            musicKick.volume = 0.0f;
        }
    }
}
