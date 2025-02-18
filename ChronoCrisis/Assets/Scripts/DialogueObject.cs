
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialoguObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;

    public string[] Dialogue=>dialogue;

    public Response[] Responses => responses;
}
