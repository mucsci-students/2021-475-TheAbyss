using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBlock : MonoBehaviour
{
    public GameObject blockedArea3;
    public GameObject areaBefore2;
    public GameObject areaBefore1;
    public GameObject inTrench;
    private MessagesMap mm;
   // private secondMapScript s;
    // Start is called before the first frame update
    void Start()
    {
        mm = FindObjectOfType<MessagesMap>();
       // s = FindObjectOfType<secondMapScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            blockedArea3.SetActive(false);
            // map prompt
            mm.UpdateTextToKey("Map2");
            mm.OpenCurrentMessage();
          //  s.hasSeenSecondMap = true;
            areaBefore2.SetActive(true);
            inTrench.SetActive(false);
            areaBefore1.SetActive(false);
        }
    }
}
