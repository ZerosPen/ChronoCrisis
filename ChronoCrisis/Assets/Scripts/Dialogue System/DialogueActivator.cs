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
    private void OnTriggEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            player.Interactable = this;
        }
    }
    private void OniggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            if(player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
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
