using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueData", menuName = "Dialogue/NPC Dialogue Data")]
public class NPCDialogueData : ScriptableObject
{
    public string npcID;               
    public string currentDialogueID;  
}
