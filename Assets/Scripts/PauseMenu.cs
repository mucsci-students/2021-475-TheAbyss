using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public MessageDisplay messageDisplay;
    public GameObject pauseMenu;
    public GameObject finalWarning;

    public PopUpText currentPopup;
    public Text textbox;
    public Image portrait;
    public Image fullscreenPortrait;

    private bool isPaused = false;

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && !messageDisplay.hasMessageOpen())
        {
            PauseGame();
        }
    }

    public bool isGamePaused()
    {
        return isPaused;
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        SetPauseMenuMessage();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetPauseMenuMessage()
    {
        currentPopup = messageDisplay.popUpText;
        if(currentPopup.portraitIsFullscreen)
        {
            fullscreenPortrait.gameObject.SetActive(true);
            fullscreenPortrait.sprite = currentPopup.portrait;
            textbox.text = "";
            portrait.sprite = null;
        }
        else
        {
            fullscreenPortrait.gameObject.SetActive(false);
            textbox.text = currentPopup.message;
            portrait.sprite = currentPopup.portrait;
        }
    }

    public void EnableFinalWarningPanel()
    {
        pauseMenu.SetActive(false);
        finalWarning.SetActive(true);
    }

    public void DisableFinalWarningPanel()
    {
        pauseMenu.SetActive(true);
        finalWarning.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScreen");
    }
}
