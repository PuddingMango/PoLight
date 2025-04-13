using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    int jumpCount = 0;  //���� ���� Ƚ��

    const float SPEED_JUMP = 7.0f;
    const int MAX_JUMP_COUNT = 2;  //�ִ� ���� ���� Ƚ�� (���� ����)

    const float SPEED_WALK = 3.0f;
    const float SPEED_RUN = 6.0f;
    const float DOUBLE_TAP_TIME = 0.3f;  //������ �ν� �ð� ����

    Rigidbody2D rb;

    bool leftPressed = false;
    bool rightPressed = false;

    bool isRunningLeft = false;
    bool isRunningRight = false;

    float lastLeftTapTime = -1f;
    float lastRightTapTime = -1f;

    bool isGrounded = false;  //�ٴڿ� ����ִ��� ����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            float moveSpeed = SPEED_WALK;  //�⺻ �ӵ� = �ȱ� �ӵ�
            Vector2 pos = transform.position;

            //���� �̵�
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastLeftTapTime < DOUBLE_TAP_TIME)  // ���� �� ����: ������ �Է� �ð��� ���� �ð� ��
                {
                    isRunningLeft = true;
                }
                lastLeftTapTime = Time.time;  //������ �Է� �ð� ����
                leftPressed = true;

            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                leftPressed = false;
                isRunningLeft = false;
            }
            if (leftPressed)
            {
                //�޸��� ���̸� �ӵ� ����
                moveSpeed = isRunningLeft ? SPEED_RUN : SPEED_WALK;
                pos.x -= moveSpeed * Time.deltaTime;
            }

            //������ �̵�
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

            transform.position = pos;  //���� ��ġ �ݿ�

            //���� + ���� ����� ����
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

    // === �ɾ���� ���θ� ��ȯ (�ִϸ��̼ǿ��� ���) ===
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

    // === �ٴڿ� ����� �� ===
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  //�ٴڿ� "Ground" �±� ���̱�
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
