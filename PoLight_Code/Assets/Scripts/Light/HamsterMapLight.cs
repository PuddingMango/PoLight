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
    public float globalLightFadeDuration = 4f;
    public float globalTargetIntensity = 1f;

    private void Start()
    {
        if (playerLight != null) playerLight.intensity = 0f;
        if (globalLight != null)
        {
            globalLight.intensity = 0f;
            globalLight.color = new Color(0, 0, 0); // 시작 시 어두운 색
        }

        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        PlaySound(fallSound, fallVolume);
        yield return new WaitForSeconds(fallSound.length);

        PlaySound(thudSound, thudVolume);
        yield return new WaitForSeconds(thudSound.length);

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
            StartCoroutine(FadeGlobalAndFadeOutPlayer());
    }

    private IEnumerator FadeGlobalAndFadeOutPlayer()
    {
        Coroutine global = StartCoroutine(FadeInGlobalLightCoroutine());

        if (playerLight != null)
            StartCoroutine(FadeOutPlayerLight());

        yield return global;
    }

    private IEnumerator FadeInGlobalLightCoroutine()
    {
        float startIntensity = globalLight.intensity;
        Color startColor = globalLight.color;
        Color targetColor = Color.white;

        float elapsed = 0f;
        while (elapsed < globalLightFadeDuration)
        {
            float t = elapsed / globalLightFadeDuration;

            globalLight.intensity = Mathf.Lerp(startIntensity, globalTargetIntensity, t);
            globalLight.color = Color.Lerp(startColor, targetColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        globalLight.intensity = globalTargetIntensity;
        globalLight.color = targetColor;
    }

    private IEnumerator FadeOutPlayerLight()
    {
        float start = playerLight.intensity;
        float elapsed = 0f;

        while (elapsed < globalLightFadeDuration)
        {
            float t = elapsed / globalLightFadeDuration;
            playerLight.intensity = Mathf.Lerp(start, 0f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerLight.intensity = 0f;
    }
}
