using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    public DialogueUI dialogueUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueUI.ShowDialogue(dialogueObject);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out CirclePlayer circlePlayer))
        {
             if (circlePlayer.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
           {

            circlePlayer.Interactable = null;

           }
         }
    }
    public void Interact(CirclePlayer circlePlayer)
    {
        circlePlayer.DialogueUI.ShowDialogue(dialogueObject);
    }
}
