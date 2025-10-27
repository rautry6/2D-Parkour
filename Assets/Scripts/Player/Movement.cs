using System;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float speed = 5;
    private int direction = 1;

    public int Direction => direction;

    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartMovement();
    }

    void OnEnable()
    {
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.OnTransitionStart += StopMovement;
            TransitionManager.Instance.OnTransitionEnd += StartMovement;
        }
    }

    void OnDisable()
    {
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.OnTransitionStart -= StopMovement;
            TransitionManager.Instance.OnTransitionEnd -= StartMovement;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeDirection()
    {
        direction *= -1;
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Check if the contact normal is predominantly horizontal
            // A normal of (1, 0) or (-1, 0) indicates a vertical wall
            if (Mathf.Abs(contact.normal.x) > 0.9f && Mathf.Abs(contact.normal.y) < 0.1f)
            {
                ChangeDirection();
                return;
            }
        }
    }

    void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    void StartMovement()
    {
        direction = 1;
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
    }
}
