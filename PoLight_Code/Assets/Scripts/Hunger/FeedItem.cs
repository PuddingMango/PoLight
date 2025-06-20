using UnityEngine;

public class FeedItem : MonoBehaviour
{
    [Header("사료 설정")]
    public bool isSpoiled = false;
    public float recoveryAmount = 20f;
    public float maxReductionAmount = 10f;

    [Header("효과음 클립")]
    public AudioClip openCanSound;
    public AudioClip poisonedSound;
    public AudioClip healSound;

    [Header("효과음 볼륨")]
    [Range(0f, 1f)] public float openCanVolume = 1f;
    [Range(0f, 1f)] public float poisonedVolume = 1f;
    [Range(0f, 1f)] public float healVolume = 1f;

    [Header("캔 따는 소리 속도 (Pitch)")]
    [Range(0.5f, 2f)] public float openCanPitch = 1f;

    private Animator animator;
    private bool used = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used || !collision.CompareTag("Player")) return;
        used = true;

        // 캔 열리는 애니메이션 재생
        if (isSpoiled)
            animator.Play("HungerFeedBad");
        else
            animator.Play("HungerFeedGood");

        // 캔 따는 효과음 (속도 적용)
        PlaySound(openCanSound, openCanVolume, openCanPitch);

        // 상태별 효과음 (기본 속도)
        if (isSpoiled)
            PlaySound(poisonedSound, poisonedVolume);
        else
            PlaySound(healSound, healVolume);

        // 허기 시스템 반영
        HungerSystem hunger = FindFirstObjectByType<HungerSystem>();
        if (hunger != null)
        {
            if (isSpoiled)
                hunger.EatSpoiledFeed(recoveryAmount, maxReductionAmount);
            else
                hunger.EatFeed(recoveryAmount);
        }

        // Destroy는 애니메이션 이벤트에서 호출됨
    }

    // 효과음 재생 함수 (pitch 생략 시 기본값 1)
    private void PlaySound(AudioClip clip, float volume, float pitch = 1f)
    {
        if (clip == null) return;

        GameObject audioObj = new GameObject("TempAudio");
        audioObj.transform.position = transform.position;

        AudioSource source = audioObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = Mathf.Clamp01(volume);
        source.pitch = pitch;
        source.Play();

        // clip.length / pitch → pitch 빠를수록 짧게 재생되므로 그에 맞춰 제거
        Destroy(audioObj, clip.length / pitch);
    }

    // 애니메이션 이벤트로 호출
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
