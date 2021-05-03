using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuObject;
    public GameObject optionsMenuObject;
    public GameObject controlsMenuObject;

    private AudioSource mainMenuSound;

    public Animator animator;
    public Image whiteScreen;

    private bool startedFade = false;

    void Start()
    {
        mainMenuSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(startedFade)
        {
            mainMenuSound.volume = mainMenuSound.volume - Time.deltaTime * 0.8f;
        }
    }
    public void StartGame()
    {
        StartCoroutine(FadeThenLoad());
    }

    IEnumerator FadeThenLoad()
    {
        startedFade = true;
        animator.SetBool("FadeToWhite", true);
        yield return new WaitUntil(() => whiteScreen.color.a == 1 && mainMenuSound.volume <= 0.0f);
        SceneManager.LoadScene("NewMain");
    }

    public void SetMouseSensitivity(float sliderValue)
    {
        PlayerPrefs.SetFloat("mouse", sliderValue);
    }

    public void ActivateControlsMenu()
    {
        controlsMenuObject.SetActive(true);
    }

    public void DeactivateControlsMenu()
    {
        controlsMenuObject.SetActive(false);
    }

    public void ActivateMainMenu()
    {
        mainMenuObject.SetActive(true);
    }

    public void DeactivateMainMenu()
    {
        mainMenuObject.SetActive(false);
    }

    public void ActivateOptionsMenu()
    {
        optionsMenuObject.SetActive(true);
    }

    public void DeactivateOptionsMenu()
    {
        optionsMenuObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
