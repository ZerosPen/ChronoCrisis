using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;

    [SerializeField] private Sprite characterSprite;
    [SerializeField] private Image imageHolder;

    public bool IsOpen {get; private set;}

    private ResponseHandler responseHandler;

    private TypeWriter typeWriter;

    private void Start()
    {
        typeWriter = GetComponent<TypeWriter>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogueBox();
    }
    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvent(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {


        for(int i=0;i<dialogueObject.Dialogue.Length;i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            // yield return typeWriter.Run(dialogue, textLabel);
            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if(i==dialogueObject.Dialogue.Length-1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(()=>Input.GetMouseButton(0));
        }

        if(dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
        
    }
    private IEnumerator RunTypingEffect(string dialogue)
    {
        typeWriter.Run(dialogue,textLabel);
        while(typeWriter.IsRunning)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                typeWriter.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

}

