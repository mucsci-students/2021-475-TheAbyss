using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class DeathScreen : MonoBehaviour
{
    public Text deathText;

    public Animator animator;
    public Image whiteScreen;

    private AudioSource mainMenuSound;
    private bool startedFade = false;

    public AudioMixer audioMixer;

    void Start()
    {
        deathText.text = CauseOfDeath.GetCauseOfDeath();
        mainMenuSound = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        audioMixer.SetFloat("masterVolume", PlayerPrefs.GetFloat("masterVolume", 1.0f));
    }

    void Update()
    {
        if(startedFade)
        {
            mainMenuSound.volume = mainMenuSound.volume - Time.deltaTime * 0.8f;
        }
    }

    public void RestartGame()
    {
        StartCoroutine(FadeThenLoad("NewMain"));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(FadeThenLoad("TitleScreen"));
    }

    IEnumerator FadeThenLoad(string scene)
    {
        startedFade = true;
        animator.SetBool("FadeToWhite", true);
        yield return new WaitUntil(() => whiteScreen.color.a == 1 && mainMenuSound.volume <= 0.0f);
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
