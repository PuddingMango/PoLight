using UnityEngine;

public class AutoDialogueTrigger : MonoBehaviour
{
    public string startDialogueID;

    void Start()
    {
        if (!string.IsNullOrEmpty(startDialogueID))
        {
            DialogueManager.Instance.StartDialogue(startDialogueID);
        }
    }
}
