using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueShooter : MonoBehaviour
{
    public GameObject eyeOne;
    public GameObject eyeTwo;
    public GameObject mouth;

    public bool eyeOneActive = true;
    public bool eyeTwoActive = true;
    public bool mouthActive = true;
    private MessagesMap mm;
    private PlayerController pc;
    private bool messageDisplayed = false;

    public GameObject lastEncounter;
    public GameObject forest2;
    public GameObject frontTrench;

    private void Start()
    {
        mm = FindObjectOfType<MessagesMap>();
    }
    // Update is called once per frame
    void Update()
    {
        if (CheckForSolve () && !messageDisplayed)
        {
            PuzzleSolved();
        }
    }

    public void TargetHit (GameObject sender)
    {
        if (sender.Equals (eyeOne))
        {
            eyeOneActive = false;
            eyeOne.GetComponent<Light>().enabled = false;
        }
        else if (sender.Equals(eyeTwo))
        {
            eyeTwoActive = false;
            eyeTwo.GetComponent<Light>().enabled = false;
        }
        else if (sender.Equals(mouth))
        {
            mouthActive = false;
            mouth.GetComponent<Light>().enabled = false;
        }
    }

    public bool CheckForSolve ()
    {
        if (!eyeOneActive && !eyeTwoActive && !mouthActive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PuzzleSolved ()
    {
        pc = FindObjectOfType<PlayerController>();
        print("Puzzle complete.");
        pc.hasKey = true;
        print("Player has acquired the key!");
        mm.UpdateTextToKey("PuzzleTwo");
        mm.OpenCurrentMessage();
        messageDisplayed = true;
        lastEncounter.SetActive(true);
        forest2.SetActive(true);
        frontTrench.SetActive(true);
    }
}
