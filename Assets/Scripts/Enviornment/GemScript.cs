using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public bool isActive;
    public enum GemColor { GREEN, YELLOW, RED };
    public GemColor color;
    public AudioSource activeSound;

    private Light light;
    private LightPuzzleManager lpm;
    

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponentInChildren<Light>();
        light.enabled = false;
        lpm = FindObjectOfType<LightPuzzleManager>();
        activeSound = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightSwitch ()
    {
        isActive = !isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("SpotLight"))
        {
            isActive = true;
            light.enabled = true;
            activeSound.Play();
            lpm.TargetHit(this.gameObject);
            print(ColorToString());
        }
    }

    private string ColorToString ()
    {
        string s = "";

        switch (color)
        {
            case GemColor.GREEN:
                s = "GREEN";
                break;
            case GemColor.YELLOW:
                s = "YELLOW";
                break;
            case GemColor.RED:
                s = "RED";
                break;
        }

        return s;
    }

}
