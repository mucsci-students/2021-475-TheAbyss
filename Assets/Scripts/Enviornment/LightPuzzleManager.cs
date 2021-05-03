using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPuzzleManager : MonoBehaviour
{
    public GameObject redGem;
    public GameObject greenGem;
    public GameObject yellowGem;
    
    public bool redActive = false;
    public bool greenActive = false;
    public bool yellowActive = false;

    public GameObject enterKelp;
    public GameObject afterGemEncounter;
    public GameObject blockedArea2;
    public bool puzzleSolved;

    public List<GameObject> gemsActive = new List<GameObject>();
    private MessagesMap mm;
    
    //Timer
    private float timer;
    private float resetDelay = 2;
    private bool startTimer = false;

    private void Start()
    {
        mm = FindObjectOfType<MessagesMap>();
    }

    private void Update()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;
        }

        if (timer >= resetDelay)
        {
            ResetPuzzle();
            startTimer = false;
            timer = 0;
        }
    }

    private bool CheckForSolve()
    {
        bool isSolved = false;
        print("Hi, im troy mcclure and you are in the check for solve function.");
        if (greenActive && yellowActive && redActive)
        {
            if (   gemsActive[0].GetComponent<GemScript>().color == GemScript.GemColor.GREEN 
                && gemsActive[1].GetComponent<GemScript>().color == GemScript.GemColor.YELLOW 
                && gemsActive[2].GetComponent<GemScript>().color == GemScript.GemColor.RED)
            {
                isSolved = true;
                blockedArea2.SetActive(false);
                SpawnNextClue();
                afterGemEncounter.SetActive(true);
                enterKelp.SetActive(true);
            }
            else
            {
                ResetPuzzle();
            }
        }

        return isSolved;
    }

    private void SpawnNextClue ()
    {
        print("next clue!");
        mm.UpdateTextToKey("Second");
        mm.OpenCurrentMessage();
    }

    public void TargetHit(GameObject sender)
    {
        if (gemsActive.Contains(sender))
        {
            return;
        }
        print("target hit called.");
        if (sender.Equals(redGem))
        {
            redActive = true;
           //sender.GetComponent<Light>().enabled = true;
            print("sender got it down on the red gem");
            gemsActive.Add(sender);
        }
        else if (sender.Equals(yellowGem))
        {
            yellowActive = true;
            //sender.GetComponent<Light>().enabled = true;
            print("sender got it down on the yellow gem");
            gemsActive.Add(sender);
        }
        else if (sender.Equals(greenGem))
        {
            greenActive = true;
            //sender.GetComponent<Light>().enabled = true;
            print("sender got it down on the green gem");
            gemsActive.Add(sender);

         }
        
        if (gemsActive.Count == 3)
        {
            if (!CheckForSolve())
            {
                startTimer = true;
            }
        }
    }

    private void ResetPuzzle()
    {
        greenActive = false;
        redActive = false;
        yellowActive = false;

        for (int i = 0; i < gemsActive.Count; i++)
        {
            gemsActive[i].GetComponentInChildren<Light>().enabled = false;
            gemsActive[i].GetComponent<GemScript>().isActive = false;
        }
        gemsActive.Clear();


        print("puzzle failed, please try again.");
    }
}
