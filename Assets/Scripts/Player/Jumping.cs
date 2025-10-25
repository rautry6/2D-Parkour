using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{

    [SerializeField] private float jumpForce = 5;

    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float fallMultiplier = 2.5f;

    [SerializeField] private float jumpBufferTime = 0.1f;


    private bool isGrounded = true;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundCheckDistance = 0.2f;
    private bool tryingToJump = false;

    private Rigidbody2D rb;

    private Controls controls;
    private InputAction jumpAction;

    // Events
    // what type of methods can subscribe to the event
    public delegate void PlayerJummpAction();
    // event instance
    public event PlayerJummpAction OnJump;

    private bool canJump = true;

    void Awake()
    {
        controls = new Controls();
        rb = GetComponent<Rigidbody2D>();
        jumpAction = controls.Movement.Jump;

    }

    void OnEnable()
    {
        jumpAction.Enable();
        TransitionManager.Instance.OnTransitionStart += () => canJump = false;
        TransitionManager.Instance.OnTransitionEnd += () => canJump = true;
    }

    void OnDestroy()
    {
        jumpAction.Disable();
        TransitionManager.Instance.OnTransitionStart -= () => canJump = false;
        TransitionManager.Instance.OnTransitionEnd -= () => canJump = true;
    }

    void Update()
    {
        if (!canJump) return;

        tryingToJump = jumpAction.IsPressed();

        // Better jumping physics
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tryingToJump)
        {
            Jump();
        }

        GroundedCheck();
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.gravityScale = gravityScale;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            OnJump?.Invoke();
        }
    }

    void GroundedCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(JumpBufferCoroutine());
        }
    }

    void OnDrawGizmos()
    {
        Color rayColor = isGrounded ? Color.green : Color.red;
        Gizmos.color = rayColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    private IEnumerator JumpBufferCoroutine()
    {
        isGrounded = true;
        yield return new WaitForSeconds(jumpBufferTime);
        isGrounded = false;
    }

}
