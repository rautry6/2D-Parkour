using UnityEngine;

public class GravityController : MonoBehaviour
{
    private bool isGrounded = false;

    public bool IsGrounded => isGrounded;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundCheckDistance = 0.2f;

    private int gravityDirection = 1;

    public int GravityDirection => gravityDirection;

    private Rigidbody2D playerRigidbody;
    private JumpRotator jumpRotator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        jumpRotator = GetComponent<JumpRotator>();
    }

    void OnEnable()
    {
        RespawnManager.Instance.OnPlayerRespawn += ResetGravity;
    }

    void OnDisable()
    {
        RespawnManager.Instance.OnPlayerRespawn -= ResetGravity;
    }

    void FixedUpdate()
    {
        GroundedCheck();
    }

    void GroundedCheck()
    {
        // downwards check
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null)
        {
            isGrounded = true;
            gravityDirection = 1;
            if (playerRigidbody.gravityScale < 0)
            {
                playerRigidbody.gravityScale *= -1;
                jumpRotator.StopRotate();
            }
            return;
        }

        // upwards check
        hit = Physics2D.Raycast(transform.position, Vector2.up, groundCheckDistance, groundLayer);
        if (hit.collider != null)
        {
            isGrounded = true;
            gravityDirection = -1;
            if (playerRigidbody.gravityScale > 0)
            {
                playerRigidbody.gravityScale *= -1;
                jumpRotator.StopRotate();
            }
            return;
        }

        isGrounded = false;
    }

    private void ResetGravity()
    {
        gravityDirection = 1;
        if (playerRigidbody.gravityScale < 0)
        {
            playerRigidbody.gravityScale *= -1;
            jumpRotator.StopRotate();
        }
    }

    void OnDrawGizmos()
    {
        Color rayColor = isGrounded ? Color.green : Color.red;
        Gizmos.color = rayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
