using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("NPC 대화 설정 리스트")]
    public List<NPCDialogueData> dialogueDataList;

    private Dictionary<string, string> npcDialogueProgress = new Dictionary<string, string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ApplyDialogueSettings();
    }

    public void ApplyDialogueSettings()
    {
        npcDialogueProgress.Clear();

        foreach (var data in dialogueDataList)
        {
            npcDialogueProgress[data.npcID] = data.currentDialogueID;
        }
    }

    public string GetDialogueID(string npcID)
    {
        return npcDialogueProgress.ContainsKey(npcID) ? npcDialogueProgress[npcID] : null;
    }

    public void SetDialogueID(string npcID, string dialogueID)
    {
        npcDialogueProgress[npcID] = dialogueID;

        // 리스트에도 반영
        var match = dialogueDataList.Find(d => d.npcID == npcID);
        if (match != null)
        {
            match.currentDialogueID = dialogueID;
        }
    }
}
