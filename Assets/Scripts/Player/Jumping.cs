using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GravityController), typeof(JumpRotator))]
public class Jumping : MonoBehaviour
{

    [SerializeField] private float jumpForce = 5;

    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float fallMultiplier = 2.5f;

    [SerializeField] private float jumpBufferTime = 0.1f;

    private bool tryingToJump = false;
    private bool jumpCheckOverride = false;

    private Rigidbody2D rb;

    private Controls controls;
    private InputAction jumpAction;

    // Events
    // what type of methods can subscribe to the event
    public delegate void PlayerJummpAction();
    // event instance
    public event PlayerJummpAction OnJump;

    private bool canJump = true;

    private GravityController gravityController;

    void Awake()
    {
        controls = new Controls();
        rb = GetComponent<Rigidbody2D>();
        jumpAction = controls.Movement.Jump;
        gravityController = GetComponent<GravityController>();

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
        if (!gravityController.IsGrounded && DownwardsVelocityCheck())
        {
            rb.gravityScale = gravityScale * fallMultiplier * gravityController.GravityDirection;
        }
        else
        {
            rb.gravityScale = gravityScale * gravityController.GravityDirection;
        }
    }

    private bool DownwardsVelocityCheck()
    {
        if (gravityController.GravityDirection == 1)
        {
            return rb.linearVelocity.y < 0;
        }
        else
        {
            return rb.linearVelocity.y > 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tryingToJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (gravityController.IsGrounded || jumpCheckOverride)
        {
            jumpCheckOverride = false;

            rb.gravityScale = gravityScale * gravityController.GravityDirection;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * gravityController.GravityDirection * jumpForce, ForceMode2D.Impulse);

            OnJump?.Invoke();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(JumpBufferCoroutine());
        }
    }

    private IEnumerator JumpBufferCoroutine()
    {
        jumpCheckOverride = true;
        yield return new WaitForSeconds(jumpBufferTime);
        jumpCheckOverride = false;

    }

}
