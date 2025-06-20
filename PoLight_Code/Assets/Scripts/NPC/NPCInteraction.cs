using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcID;
    public GameObject eKeyIcon;

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby &&
            Input.GetKeyDown(KeyCode.E) &&
            !DialogueManager.Instance.IsTalking)
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
        {
            isPlayerNearby = true;
            UpdateEIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (eKeyIcon != null) eKeyIcon.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            UpdateEIcon();
        }
    }

    private void UpdateEIcon()
    {
        if (eKeyIcon == null) return;

        bool isMyAutoDialogue = DialogueManager.Instance.IsTalking &&
                                DialogueManager.Instance.CurrentSpeaker == npcID.ToLower();

        eKeyIcon.SetActive(!DialogueManager.Instance.IsTalking || !isMyAutoDialogue);
    }
}
