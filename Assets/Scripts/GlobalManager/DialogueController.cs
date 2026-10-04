using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    public GameObject dialogueUI;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public Transform Choicetransform;
    public GameObject ChoiceButtonPrefab;
    public Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetDialogueUI(bool show)
    {
        dialogueUI.SetActive(show);
    }

    public void SetNPC(string name, Image sprite)
    {
        nameText.text = name;
        Image NPCsprite = Instantiate(sprite, portraitImage.transform);

        animator = NPCsprite.GetComponent<Animator>();
    }

    public void SetDialogueLine(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach(Transform transform in Choicetransform) { Destroy(transform.gameObject); }
    }

    public GameObject SetChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(ChoiceButtonPrefab, Choicetransform);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);

        return choiceButton;
    }


}
