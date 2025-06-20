using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI 연결")]
    public GameObject leftBalloonUI;
    public GameObject rightBalloonUI;
    public TextMeshProUGUI leftDialogueText;
    public TextMeshProUGUI rightDialogueText;

    [Header("캐릭터 앵커")]
    public Transform playerBalloonAnchor;

    private Dictionary<string, Transform> speakerAnchors = new Dictionary<string, Transform>();
    private Dictionary<string, Animator> speakerAnimators = new Dictionary<string, Animator>();

    private DialogueLine currentLine;
    private bool isTalking = false;
    public bool IsTalking => isTalking;
    public string CurrentSpeaker => currentLine?.speaker?.Trim().ToLower();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        RegisterAnchors();
        RegisterAnimators();

        leftBalloonUI.SetActive(false);
        rightBalloonUI.SetActive(false);
    }

    void Update()
    {
        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!string.IsNullOrEmpty(currentLine.next_id))
            {
                currentLine = DialogueDatabase.GetLine(currentLine.next_id);
                if (currentLine != null)
                {
                    string speaker = currentLine.speaker.Trim().ToLower();
                    ShowBalloon(speaker, currentLine.text);
                    PlayExpression(currentLine);

                    if (currentLine.id == "scene3_90")
                    {
                        var light = FindFirstObjectByType<HamsterMapLight>();
                        light?.FadeInGlobalLight();
                    }
                }
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void StartDialogue(string startID)
    {
        currentLine = DialogueDatabase.GetLine(startID);

        if (currentLine == null)
        {
            Debug.LogError($"[DialogueManager] ID '{startID}'에 해당하는 대사가 없습니다.");
            return;
        }

        isTalking = true;
        string speaker = currentLine.speaker.Trim().ToLower();

        ShowBalloon(speaker, currentLine.text);
        PlayExpression(currentLine);

        if (SceneManager.GetActiveScene().name == "Scene3_17" && speaker == "hamster")
        {
            var light = FindFirstObjectByType<HamsterMapLight>();
            light?.FadeInGlobalLight();
        }
    }

    private void EndDialogue()
    {
        isTalking = false;
        leftBalloonUI.SetActive(false);
        rightBalloonUI.SetActive(false);
    }

    private void ShowBalloon(string speakerKey, string text)
    {
        leftBalloonUI.SetActive(false);
        rightBalloonUI.SetActive(false);

        if (speakerKey == "player")
        {
            leftBalloonUI.SetActive(true);
            leftDialogueText.text = text;

            if (playerBalloonAnchor != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(playerBalloonAnchor.position + new Vector3(0, 1.5f, 0));
                leftBalloonUI.transform.position = screenPos;
            }
        }
        else
        {
            rightBalloonUI.SetActive(true);
            rightDialogueText.text = text;

            if (speakerAnchors.TryGetValue(speakerKey, out Transform anchor) && anchor != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(anchor.position + new Vector3(0, 1.5f, 0));
                rightBalloonUI.transform.position = screenPos;
            }
        }
    }

    private void RegisterAnchors()
    {
        speakerAnchors.Clear();
        speakerAnchors["player"] = playerBalloonAnchor;
        speakerAnchors["guide"] = GameObject.Find("GuideAnchor")?.transform;
        speakerAnchors["cat"] = GameObject.Find("CatAnchor")?.transform;
        speakerAnchors["parrot"] = GameObject.Find("ParrotAnchor")?.transform;
        speakerAnchors["hamster"] = GameObject.Find("HamsterAnchor")?.transform;
    }

    private void RegisterAnimators()
    {
        speakerAnimators.Clear();

        speakerAnimators["player"] = GameObject.FindWithTag("Player")?.GetComponent<Animator>();
        speakerAnimators["guide"] = GameObject.Find("Guide")?.GetComponent<Animator>();
        speakerAnimators["cat"] = GameObject.Find("Cat")?.GetComponent<Animator>();
        speakerAnimators["parrot"] = GameObject.Find("Parrot")?.GetComponent<Animator>();
        speakerAnimators["hamster"] = GameObject.Find("Hamster")?.GetComponent<Animator>();

        foreach (var pair in speakerAnimators)
        {
            if (pair.Value == null)
                Debug.LogWarning($"[RegisterAnimators] '{pair.Key}' 애니메이터 등록 실패 (null)");
            else
                Debug.Log($"[RegisterAnimators] '{pair.Key}' 애니메이터 등록 완료");
        }
    }

    private void PlayExpression(DialogueLine line)
    {
        string key = line.speaker.Trim().ToLower();

        if (speakerAnimators.TryGetValue(key, out Animator animator))
        {
            if (animator == null)
            {
                Debug.LogWarning($"[DialogueManager] '{key}'의 애니메이터는 null입니다 (컴포넌트 없음)");
            }
            else
            {
                bool hasState = animator.HasState(0, Animator.StringToHash(line.expression));
                if (!hasState)
                    Debug.LogWarning($"[DialogueManager] '{key}' 애니메이터에 '{line.expression}' 상태가 존재하지 않음");

                Debug.Log($"[DialogueManager] '{key}' 애니메이터 → '{line.expression}' 상태 재생 시도");
                animator.Play(line.expression);
            }
        }
        else
        {
            Debug.LogWarning($"[DialogueManager] '{key}'의 애니메이터가 딕셔너리에 없음");
        }
    }
}
