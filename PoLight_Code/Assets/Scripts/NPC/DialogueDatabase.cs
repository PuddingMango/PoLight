using System.Collections.Generic;
using UnityEngine;

public class DialogueDatabase : MonoBehaviour
{
    public static Dictionary<string, DialogueLine> dialogueMap;

    void Awake()
    {
        Debug.Log("[DialogueDatabase] Awake 실행됨");

        TextAsset json = Resources.Load<TextAsset>("dialogues");
        if (json == null)
        {
            Debug.LogError("[DialogueDatabase] dialogues.json 파일을 찾을 수 없습니다!");
            return;
        }

        DialogueLine[] lines = JsonHelper.FromJson<DialogueLine>(json.text);

        dialogueMap = new Dictionary<string, DialogueLine>();
        foreach (var line in lines)
        {
            dialogueMap[line.id] = line;
        }

        Debug.Log($"[DialogueDatabase] 대사 {dialogueMap.Count}개 로드 완료");
    }


    public static DialogueLine GetLine(string id)
    {
        if (dialogueMap.ContainsKey(id))
            return dialogueMap[id];
        else
            return null;
    }
}
