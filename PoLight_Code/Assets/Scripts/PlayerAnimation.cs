using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Animator animator;
    PlayerMove playerMove;  //이동 상태를 가져오기 위한 참조
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();  //같은 오브젝트에 붙어 있는 PlayerMove 스크립트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMove != null && animator != null)
        {
            //캐릭터가 공중에 떠 있으면 점프 애니메이션 우선 출력
            bool isJumping = !playerMove.IsGrounded();
            animator.SetBool("isJumping", isJumping);

            //점프 중이 아니어야 걷기 / 달리기 판별
            if (isJumping)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
            }
            else
            {
                //점프 중이면 걷기 / 달리기 false로 설정 (우선순위 강제)
                bool isRunning = playerMove.IsRunning();
                bool isWalking = !isRunning && playerMove.IsWalking();

                animator.SetBool("isRunning", isRunning);
                animator.SetBool("isWalking", isWalking);
            }

            //좌우 반전 처리
            if (playerMove.IsMovingLeft())
            {
                spriteRenderer.flipX = true;  //왼쪽
            }
            else if (playerMove.IsMovingRight())
            {
                spriteRenderer.flipX = false;  //오른쪽
            }
        }
    }
}