using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Message")]
public class PopUpText : ScriptableObject
{
    [Tooltip("Supports rich text. For example, <color=green>This text is green</color> will create green text.")]
    [TextArea(15,20)]
    public string message;
    public Sprite portrait;
    public bool portraitIsFullscreen;
}
