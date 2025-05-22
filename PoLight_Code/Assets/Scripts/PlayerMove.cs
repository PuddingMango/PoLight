using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    int jumpCount = 0;

    const float SPEED_JUMP = 7.0f;
    const int MAX_JUMP_COUNT = 1;

    const float SPEED_WALK = 4.0f;
    const float SPEED_RUN = 6.0f;
    const float DOUBLE_TAP_TIME = 0.3f;

    Rigidbody2D rb;

    bool leftPressed = false;
    bool rightPressed = false;

    bool isRunningLeft = false;
    bool isRunningRight = false;

    float lastLeftTapTime = -1f;
    float lastRightTapTime = -1f;

    bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb != null)
        {
            float moveSpeed = SPEED_WALK;
            Vector2 pos = transform.position;

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - lastLeftTapTime < DOUBLE_TAP_TIME)
                    isRunningLeft = true;

                lastLeftTapTime = Time.time;
                leftPressed = true;
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                leftPressed = false;
                isRunningLeft = false;
            }

            if (leftPressed)
            {
                moveSpeed = isRunningLeft ? SPEED_RUN : SPEED_WALK;
                pos.x -= moveSpeed * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - lastRightTapTime < DOUBLE_TAP_TIME)
                    isRunningRight = true;

                lastRightTapTime = Time.time;
                rightPressed = true;
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                rightPressed = false;
                isRunningRight = false;
            }

            if (rightPressed)
            {
                moveSpeed = isRunningRight ? SPEED_RUN : SPEED_WALK;
                pos.x += moveSpeed * Time.deltaTime;
            }

            transform.position = pos;

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpCount < MAX_JUMP_COUNT)
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
