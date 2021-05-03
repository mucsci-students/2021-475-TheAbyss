using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public string KeyTextWarning;
    public string KeySharkPoint;
    
    public bool teleportShark = true;
    public float newSharkPatrolRadius = 20f;

    private MessagesMap mm;
    private SharkManager sharkManager;

    private bool hasGottenThisPrompt = false;

    void Start()
    {
        mm = FindObjectOfType<MessagesMap>();
        sharkManager = FindObjectOfType<SharkManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !hasGottenThisPrompt)
        {
            if(KeyTextWarning != "")
            {
                mm.UpdateTextToKey(KeyTextWarning);
                mm.OpenCurrentMessage();
            }
            
            sharkManager.SetPatrolRadius(newSharkPatrolRadius);

            if(teleportShark && KeySharkPoint != "")
            {
                sharkManager.TeleportSharkToKeyPoint(KeySharkPoint);
            }
            else if(!teleportShark && KeySharkPoint != "")
            {
                sharkManager.SetSharkInterestPointToKey(KeySharkPoint);
            }
            
            hasGottenThisPrompt = true;
        }
    }
}
