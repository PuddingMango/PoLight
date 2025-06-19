using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcID;
    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            string dialogueID = GameManager.Instance.GetDialogueID(npcID);
            if (!string.IsNullOrEmpty(dialogueID))
            {
                DialogueManager.Instance.StartDialogue(dialogueID); 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = false;
    }
}
