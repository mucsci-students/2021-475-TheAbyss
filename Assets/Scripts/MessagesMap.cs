using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MessagesMap : MonoBehaviour
{
    [Serializable]
    public class Message
    {
        public string key;
        public PopUpText popUp;
    }

    [SerializeField]
    public Message[] messages;
    private Dictionary<string, PopUpText> m_messages;

    public MessageDisplay messageDisplayer;

    void Start()
    {
        m_messages = new Dictionary<string, PopUpText>();
        foreach(Message m in messages)
        {
            string newKey = m.key;
            PopUpText newPopUp = m.popUp;
            m_messages.Add(newKey, newPopUp);
        }
    }

    public void UpdateTextToKey(string key)
    {
        PopUpText newPopUp;
        bool success = m_messages.TryGetValue(key, out newPopUp);

        if(success)
        {
            if(newPopUp.portraitIsFullscreen)
            {
                messageDisplayer.UpdateDisplayToFullscreenImage(newPopUp);
            }
            else
            {
                messageDisplayer.UpdateDisplayText(newPopUp);
            }
        }
        else
        {
            Debug.LogError("The key \"" + key + "\" does not exist under the MessageMap. Please add a PopUpText object with a key to the MessageMap.");
        }
    }

    public void OpenCurrentMessage()
    {
        messageDisplayer.OpenCurrentMessage();
    }
}
