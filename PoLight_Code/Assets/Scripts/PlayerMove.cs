using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    int jumpCount = 0;  //현재 점프 횟수

    const float SPEED_JUMP = 7.0f;
    const int MAX_JUMP_COUNT = 2;  //최대 점프 가능 횟수 (더블 점프)

    const float SPEED_WALK = 3.0f;
    const float SPEED_RUN = 6.0f;
    const float DOUBLE_TAP_TIME = 0.3f;  //더블탭 인식 시간 간격

    Rigidbody2D rb;

    bool leftPressed = false;
    bool rightPressed = false;

    bool isRunningLeft = false;
    bool isRunningRight = false;

    float lastLeftTapTime = -1f;
    float lastRightTapTime = -1f;

    bool isGrounded = false;  //바닥에 닿아있는지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            float moveSpeed = SPEED_WALK;  //기본 속도 = 걷기 속도
            Vector2 pos = transform.position;

            //왼쪽 이동
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastLeftTapTime < DOUBLE_TAP_TIME)  // 더블 탭 감지: 마지막 입력 시간과 현재 시간 비교
                {
                    isRunningLeft = true;
                }
                lastLeftTapTime = Time.time;  //마지막 입력 시간 갱신
                leftPressed = true;

            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                leftPressed = false;
                isRunningLeft = false;
            }
            if (leftPressed)
            {
                //달리기 중이면 속도 증가
                moveSpeed = isRunningLeft ? SPEED_RUN : SPEED_WALK;
                pos.x -= moveSpeed * Time.deltaTime;
            }

            //오른쪽 이동
            if (Input .GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastRightTapTime < DOUBLE_TAP_TIME)
                {
                    isRunningRight = true;
                }
                lastRightTapTime = Time.time;
                rightPressed = true;

            }
            if (Input.GetKeyUp (KeyCode.D))
            {
                rightPressed= false;
                isRunningRight = false;
            }
            if (rightPressed)
            {
                moveSpeed = isRunningRight ? SPEED_RUN : SPEED_WALK;
                pos.x += moveSpeed * Time.deltaTime;
            }

            transform.position = pos;  //실제 위치 반영

            //점프 + 땅에 닿았을 때만
            if ((Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpCount < MAX_JUMP_COUNT)
            {
                Vector2 moveVelocity = rb.linearVelocity;
                moveVelocity.y = SPEED_JUMP;
                rb.linearVelocity = moveVelocity;

                jumpCount++;
            }
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    // === 걸어가는지 여부를 반환 (애니메이션에서 사용) ===
    public bool IsWalking()
    {
        return (leftPressed || rightPressed) && !IsRunning();
    }
    
    public bool IsRunning()
    {
        return isRunningLeft || isRunningRight;
    }

    public bool IsJumping()
    {
        return !isGrounded;
    }

    public bool IsMovingLeft()
    {
        return leftPressed;
    }

    public bool IsMovingRight()
    {
        return rightPressed;
    }

    // === 바닥에 닿았을 때 ===
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  //바닥에 "Ground" 태그 붙이기
        {
            isGrounded= true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded= false;
        }
    }
}
