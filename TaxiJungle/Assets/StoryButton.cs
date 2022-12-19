using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[CreateAssetMenu(fileName = "TextMessage")]
public class StoryButton : ScriptableObject
{
    public string _massageName;
    [TextArea] public string messageContent;
}
