using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class HamsterMapLight : MonoBehaviour
{
    [Header("Intro Audio")]
    public AudioClip fallSound;
    public AudioClip thudSound;
    [Range(0f, 1f)] public float fallVolume = 1f;
    [Range(0f, 1f)] public float thudVolume = 1f;

    [Header("Player Light")]
    public Light2D playerLight;
    public float playerLightFadeDuration = 1.5f;
    public float playerTargetIntensity = 1.2f;

    [Header("Global Light")]
    public Light2D globalLight;
    public float globalLightFadeDuration = 2f;
    public float globalTargetIntensity = 1f;

    private void Start()
    {
        if (playerLight != null) playerLight.intensity = 0f;
        if (globalLight != null) globalLight.intensity = 0f;

        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        // 떨어지는 소리
        PlaySound(fallSound, fallVolume);
        yield return new WaitForSeconds(fallSound.length);

        // 쿵 소리
        PlaySound(thudSound, thudVolume);
        yield return new WaitForSeconds(thudSound.length);

        // 전체 맵 서서히 밝아짐
        if (globalLight != null)
            yield return StartCoroutine(FadeInGlobalLightCoroutine());

        // 플레이어 주변도 점점 밝아짐
        if (playerLight != null)
            yield return StartCoroutine(FadeInPlayerLight());
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null) return;

        GameObject temp = new GameObject("TempSound");
        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Destroy(temp, clip.length);
    }

    private IEnumerator FadeInPlayerLight()
    {
        float elapsed = 0f;
        while (elapsed < playerLightFadeDuration)
        {
            float t = elapsed / playerLightFadeDuration;
            playerLight.intensity = Mathf.Lerp(0f, playerTargetIntensity, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        playerLight.intensity = playerTargetIntensity;
    }

    public void FadeInGlobalLight()
    {
        if (globalLight != null)
            StartCoroutine(FadeInGlobalLightCoroutine());
    }

    private IEnumerator FadeInGlobalLightCoroutine()
    {
        float start = globalLight.intensity;
        float elapsed = 0f;

        while (elapsed < globalLightFadeDuration)
        {
            float t = elapsed / globalLightFadeDuration;
            globalLight.intensity = Mathf.Lerp(start, globalTargetIntensity, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        globalLight.intensity = globalTargetIntensity;
    }
}
