using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{

    public string npcName;
    public Image npcSprite;

    [Header("设置对话")]
    public string[] dialogueLines;
    public DialogueChoice[] dialogueChoices;
    public bool autoDialogue;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float TypingSpeed = 0.1f;

    [Header("设置声音")]
    public AudioClip voiceSound;



}

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex;
    public string[] Choices;
    public int[] nextDialogueIndexes;

}


