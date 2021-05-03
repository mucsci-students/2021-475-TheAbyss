using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCaveScript : MonoBehaviour
{
    public MessagesMap mm;
    public SharkManager sharkManager;

    public bool hasSeenFirstMap = false;
    private bool hasGottenPrompt = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !hasGottenPrompt)
        {
            if(hasSeenFirstMap)
            {
                mm.UpdateTextToKey("FirstSharkWarning");
                mm.OpenCurrentMessage();
                sharkManager.SetPatrolRadius(40);
                sharkManager.TeleportSharkToKeyPoint("StartingArea");
                hasGottenPrompt = true;
            }
        }
    }
}
