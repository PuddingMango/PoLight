using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Animator animator;
    PlayerMove playerMove;  //�̵� ���¸� �������� ���� ����
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();  //���� ������Ʈ�� �پ� �ִ� PlayerMove ��ũ��Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMove != null && animator != null)
        {
            bool isJumping = !playerMove.IsGrounded();
            animator.SetBool("isJumping", isJumping);

            if (isJumping)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
            }
            else
            {
                bool isRunning = playerMove.IsRunning();
                bool isWalking = !isRunning && playerMove.IsWalking();

                animator.SetBool("isRunning", isRunning);
                animator.SetBool("isWalking", isWalking);
            }

            if (playerMove.IsMovingLeft())
            {
                spriteRenderer.flipX = true;  //����
            }
            else if (playerMove.IsMovingRight())
            {
                spriteRenderer.flipX = false;  //������
            }
        }
    }
}