using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;

    DialogueController dialogueUI;

    int dialogueIndex;
    bool isTyping, isDialogueActive;

    void Start()
    {
        dialogueUI = DialogueController.Instance;
    }


    public bool CanInteracte()
    {
        return !isDialogueActive;
    }

    public void Interacte()
    {
        //有对话，且没有因为打开菜单键暂停游戏；
        if (dialogueData == null || (PauseController.IsGamePaused && !isDialogueActive))
        {
            return;
        }
        

        if (isDialogueActive)
        {
            NextDialogue();
        }
        else
        {
            startDialogue();
        }

    }

    void Update()
    {

        if (dialogueUI.dialogueUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Interacte();
            }
        }
    }


    void startDialogue()
    {
        //打开对话框，对话从零开始；
        isDialogueActive = true;
        dialogueIndex = 0;

        //设置对话框头像;
        dialogueUI.SetNPC(dialogueData.npcName, dialogueData.npcSprite);

        dialogueUI.SetDialogueUI(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());

    }


    void NextDialogue()
    {
        //如果正在打字，快进完成；
        if (isTyping)
        {
            dialogueUI.SetDialogueLine(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            dialogueUI.animator.SetBool("Speak", isTyping);
        }
        //如果还有下一条可供索引；
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        //关掉对话；
        else
        {
            endDialogue();
        }



    }

    void endDialogue()
    {

        isDialogueActive = false;
        dialogueUI.SetDialogueLine("");

        dialogueUI.SetDialogueUI(false);
        PauseController.SetPause(false);

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueLine("");
        dialogueUI.animator.SetBool("Speak", isTyping);


        foreach(char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            if (!isTyping)
            {
                yield break;
            }

            dialogueUI.SetDialogueLine(dialogueUI.dialogueText.text += letter);
            SoundEffectManager.Instance.PlaySpeakAudio(dialogueData.voiceSound);
            yield return new WaitForSeconds(dialogueData.TypingSpeed);
        }

        isTyping = false;
        dialogueUI.animator.SetBool("Speak", isTyping);

        //if (dialogueData.autoProgressLines.Length < dialogueIndex && dialogueData.autoDialogue && dialogueData.autoProgressLines[dialogueIndex])
        //{
        //    yield return new WaitForSeconds(dialogueData.autoProgressDelay);
        //    NextDialogue();
        //}

    }


}
