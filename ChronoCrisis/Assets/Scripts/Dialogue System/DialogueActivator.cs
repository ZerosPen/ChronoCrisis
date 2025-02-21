using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject=dialogueObject;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            Debug.Log("Player enter Interactable zone");
            player.Interactable = this;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            if(player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                Debug.Log("Player out Interactable zone");
                player.Interactable = null;
            }
        }
    }
    public void Interact(PlayerController player)
    {
        
        foreach (DialogueResponseEvent responseEvents in GetComponents<DialogueResponseEvent>())
        {
            if(responseEvents.DialogueObject == dialogueObject)
            {
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}
