using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMapManager : MonoBehaviour
{
    public GameObject blockedArea1;
    private MessagesMap mm;
    private FirstCaveScript f;
    // Start is called before the first frame update
    void Start()
    {
        mm = FindObjectOfType<MessagesMap>();
        f = FindObjectOfType<FirstCaveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            // map prompt
            mm.UpdateTextToKey("Map1");
            mm.OpenCurrentMessage();
            // area trigger
            if(f != null)
                f.hasSeenFirstMap = true;
            //destroy self
            blockedArea1.SetActive(false);
        }
    }
}
