using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float maxAirSupply;
    public float airDepletionRate;

    public Image airSupplyImage;

    public GameObject WhiteFadeObject;
    public AudioMixer mixer;

    public AudioSource lowAirSFX;
    public AudioSource screamSFX;

    private Image whiteScreen;
    private Animator whiteScreenAnim;
    private bool startedFade = false;
    private float volumeFadeSpeed = 60;

    private float currentAirSupply;

    private PlayerController playerController;

    void Start()
    {
        WhiteFadeObject = GameObject.Find("WhiteSquare");
        currentAirSupply = maxAirSupply;
        playerController = GetComponent<PlayerController>();
        whiteScreen = WhiteFadeObject.GetComponent<Image>();
        whiteScreenAnim = WhiteFadeObject.GetComponent<Animator>();
    }

    void Update()
    {
        if(startedFade)
        {
            float output;
            mixer.GetFloat("masterVolume", out output);
            mixer.SetFloat("masterVolume", output - Time.deltaTime * volumeFadeSpeed);
        }

        float factor;
        if(playerController.isInWater())
        {
            currentAirSupply = Mathf.Clamp((currentAirSupply - airDepletionRate * Time.deltaTime), 0.0f, maxAirSupply);
            factor = currentAirSupply / maxAirSupply;
            airSupplyImage.fillAmount = Mathf.Lerp(0.0f, 1.0f, factor);
            if(currentAirSupply <= 0.0f)
            {
                KillPlayer("Asphyxiation");
            }
        }
        else
        {
            currentAirSupply = Mathf.Clamp(currentAirSupply + airDepletionRate * Time.deltaTime, 0.0f, maxAirSupply);
            factor = currentAirSupply / maxAirSupply;
            airSupplyImage.fillAmount = Mathf.Lerp(0.0f, 1.0f, factor);
        }

        if(currentAirSupply <= maxAirSupply * 0.33f)
        {
            airSupplyImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
            lowAirSFX.volume = 1.0f;
        }
        else
        {
            airSupplyImage.color = Color.white;
            lowAirSFX.volume = 0.0f;
        }
    }

    public void KillPlayer(string causeOfDeath)
    {
        CauseOfDeath.UpdateCauseOfDeath(causeOfDeath);
        if(causeOfDeath == "Eaten by shark" && !startedFade)
        {
            screamSFX.Play();
        }
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        startedFade = true;
        whiteScreenAnim.SetBool("FadeToWhite", true);
        playerController.enabled = false;
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("DeathScreen");
    }

    public void RefillAir(float air)
    {
        currentAirSupply = Mathf.Clamp(currentAirSupply + air, 0.0f, maxAirSupply);
    }
}
