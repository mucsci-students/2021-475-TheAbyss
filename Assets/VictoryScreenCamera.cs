using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreenCamera : MonoBehaviour
{

    public Animator animator;
    public Image whiteScreen;
    
    private bool startedFade = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void GoToMainMenu()
    {
        StartCoroutine(FadeThenLoad("TitleScreen"));
    }

    public void GoToGame()
    {
        StartCoroutine(FadeThenLoad("NewMain"));
    }

    IEnumerator FadeThenLoad(string scene)
    {
        startedFade = true;
        animator.SetBool("FadeToWhite", true);
        yield return new WaitUntil(() => whiteScreen.color.a == 1);
        SceneManager.LoadScene(scene);
    }
}
