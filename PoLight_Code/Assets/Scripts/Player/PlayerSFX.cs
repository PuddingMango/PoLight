using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip jumpClip;

    [Range(0f, 1f)] public float walkVolume = 1f;
    [Range(0f, 1f)] public float runVolume = 1f;
    [Range(0f, 1f)] public float jumpVolume = 1f;

    private PlayerMove playerMove;
    private bool isGrounded = true;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (audioSource.isPlaying && audioSource.clip == jumpClip)
            return; // ���� ���� ��� ���� ���� �ȱ� �Ҹ� �� ��

        HandleFootstepSounds();
    }

    void HandleFootstepSounds()
    {
        if (isGrounded && playerMove.isMoving)
        {
            AudioClip targetClip = playerMove.isRunning ? runClip : walkClip;
            float targetPitch = playerMove.isRunning ? 2.0f : 1.4f;
            float targetVolume = playerMove.isRunning ? runVolume : walkVolume;

            if (!audioSource.isPlaying || audioSource.clip != targetClip)
            {
                audioSource.clip = targetClip;
                audioSource.pitch = targetPitch;
                audioSource.volume = targetVolume;  // ���� ����
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip != jumpClip)
            {
                audioSource.Stop();
            }
        }
    }

    public void PlayJumpSound()
    {
        if (jumpClip != null)
        {
            audioSource.Stop();
            audioSource.clip = jumpClip;
            audioSource.volume = jumpVolume;  // ���� ���� ����
            audioSource.loop = false;
            audioSource.pitch = 1f;
            audioSource.Play();
        }
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}
