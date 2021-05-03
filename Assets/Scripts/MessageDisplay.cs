using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    public PopUpText popUpText;

    public Text messageText;
    public Image portrait;
    public Image fullscreenPortrait;

    public Animator anim;

    private bool isMessageOpen = false;

    void Start()
    {
        UpdateDisplayText(popUpText);
        OpenCurrentMessage();
    }

    void Update()
    {
        if(isMessageOpen && Input.GetKeyDown(KeyCode.C))
        {
            CloseCurrentMessage();
        }
    }

    public void OpenCurrentMessage()
    {
        StartCoroutine(OpenMessage());
    }

    IEnumerator OpenMessage()
    {
        anim.SetBool("CloseMessage", false);
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0.0f;
        isMessageOpen  = true;
    }

    public void CloseCurrentMessage()
    {
        isMessageOpen = false;
        Time.timeScale = 1.0f;
        anim.SetBool("CloseMessage", true);
    }

    public void UpdateDisplayText(PopUpText newPopup)
    {
        fullscreenPortrait.gameObject.SetActive(false);
        messageText.text = newPopup.message;
        portrait.sprite = newPopup.portrait;
        popUpText = newPopup;

        if(newPopup.portraitIsFullscreen)
        {
            UpdateDisplayToFullscreenImage(newPopup);
        }
    }

    public void UpdateDisplayToFullscreenImage(PopUpText newPopup)
    {
        popUpText = newPopup;
        messageText.text = "";
        portrait.sprite = null;

        fullscreenPortrait.gameObject.SetActive(true);
        fullscreenPortrait.sprite = newPopup.portrait;
    }

    public bool hasMessageOpen()
    {
        return isMessageOpen;
    }
}
