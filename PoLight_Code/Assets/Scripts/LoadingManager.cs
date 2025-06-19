using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [Header("8칸 로딩바")]
    public Image[] cells; // 셀 10개
    public Sprite emptySprite;
    public Sprite filledSprite;

    [Header("다음 씬 이름")]
    private string nextScene;

    void Start()
    {
        nextScene = SceneTransitionData.nextSceneName;
        StartCoroutine(LoadAsyncScene());
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) Destroy(player); 
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            UpdateLoadingBar(progress);

            if (progress >= 1f)
            {
                yield return new WaitForSeconds(0.5f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    void UpdateLoadingBar(float progress)
    {
        int activeCount = Mathf.FloorToInt(progress * cells.Length);

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].sprite = (i < activeCount) ? filledSprite : emptySprite;
        }
    }
}

