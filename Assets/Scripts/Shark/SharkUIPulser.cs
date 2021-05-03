using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkUIPulser : MonoBehaviour
{
    private Image shark;
    private float lastColorChangeTime;

    private Color target;
    private Color original;

    private bool pulse = false;
    private float i = 0.0f;

    void Start()
    {
        shark = GetComponent<Image>(); 
        original = Color.white;
        target = Color.red;
    }

    void Update()
    {
        if(pulse)
        {
            i += Time.deltaTime;
            shark.color = Color.Lerp(original, target, Mathf.PingPong(i * 2, 1));
            if(i >= 1.0f)
            {
                i = 0;
                pulse = false;
            }
        }
    }

    public void Pulse()
    {
        pulse = true;
    }
}
