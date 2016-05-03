using UnityEngine;
using Prime31;
using System;

/// <summary>
/// This is an extension to Prime31's CharacterController2D for platformer controls
/// </summary>
[RequireComponent(typeof(CharacterController2D))]
public class CharacterPlatformer : MonoBehaviour
{
    private const float DropDownRate = 3.0f;

    [Tooltip("Speed at which the character can run left and right")]
    [SerializeField]
    [Range(0, 25)]
    private float runSpeed = 8.0f;

    [Tooltip("How fast the character can change directions")]
    [SerializeField]
    [Range(0, 100)]
    private float groundDamping = 20.0f;

    [Tooltip("How fast the character can change direction while in the air")]
    [SerializeField]
    [Range(0, 100)]
    private float inAirDamping = 5.0f;

    [Tooltip("Percentage of Max Jump Height that is the base jump")]
    [SerializeField]
    [Range(0, 1)]
    private float startingJumpHeight = 0.5f;

    [Tooltip("How high the character can jump")]
    [SerializeField]
    [Range(0, 25)]
    private float maxJumpHeight = 5.0f;

    [Tooltip("How fast the jump force increases")]
    [SerializeField]
    [Range(0, 1)]
    private float jumpIncreaseRate = 0.5f;

    public event Action<RaycastHit2D> onControllerCollidedEvent;
    public event Action<Collider2D> onTriggerEnterEvent;
    public event Action<Collider2D> onTriggerStayEvent;
    public event Action<Collider2D> onTriggerExitEvent;

    private float oldAppliedJumpPower;
    private float appliedJumpPower;
    private float horizontalSpeed;
    private CharacterController2D characterController2D;
    private RaycastHit2D lastControllerColliderHit;
    private Vector2 velocity;
    private Rigidbody2D body2D;
    private bool jumping;


    void Awake()
    {
        characterController2D = GetComponent<CharacterController2D>();
        body2D = GetComponent<Rigidbody2D>();
        characterController2D.onControllerCollidedEvent += onCharacterControllerCollider;
        characterController2D.onTriggerEnterEvent += onCharacterTriggerEnterEvent;
        characterController2D.onTriggerStayEvent += onCharacterTriggerStayEvent;
        characterController2D.onTriggerExitEvent += onCharacterTriggerExitEvent;
    }

    void onCharacterControllerCollider(RaycastHit2D hit)
    {
        if (onControllerCollidedEvent != null)
        {
            onControllerCollidedEvent(hit);
        }
    }


    void onCharacterTriggerEnterEvent(Collider2D collider2D)
    {
        if (onTriggerEnterEvent != null)
        {
            onTriggerEnterEvent(collider2D);
        }
    }


    void onCharacterTriggerStayEvent(Collider2D collider2D)
    {
        if (onTriggerStayEvent != null)
        {
            onTriggerStayEvent(collider2D);
        }
    }

    void onCharacterTriggerExitEvent(Collider2D collider2D)
    {
        if (onTriggerExitEvent != null)
        {
            onTriggerExitEvent(collider2D);
        }
    }

    void Update()
    {
        if (characterController2D.isGrounded)
        {
            velocity.y = 0;
        }
        if (jumping)
        {
            ApplyJump();
        }
        ApplyHorizontalMovement();
        ApplyGravity();
        ApplyVelocity();
    }

    /// <summary>
    /// Moves the character left or right
    /// </summary>
    public void Move(float horizontalMovement)
    {
        horizontalSpeed = horizontalMovement;
        if (horizontalSpeed != 0 && Mathf.Sign(horizontalSpeed) != Mathf.Sign(transform.localScale.x))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    /// <summary>
    /// Makes the character jump if they are able
    /// </summary>
    public void Jump()
    {
        if (!jumping && characterController2D.isGrounded)
        {
            jumping = true;
            appliedJumpPower = maxJumpHeight * startingJumpHeight;
            oldAppliedJumpPower = 0;
        }
        else if (jumping)
        {
            appliedJumpPower += 1.0f / maxJumpHeight / jumpIncreaseRate;
        }
    }

    /// <summary>
    /// Drops the character down on a One Way Platform
    /// </summary>
    public void DropDownPlatform()
    {
        if (characterController2D.isGrounded)
        {
            transform.Translate(0, -characterController2D.skinWidth, 0);
            velocity.y *= DropDownRate;
            characterController2D.ignoreOneWayPlatformsThisFrame = true;
        }
    }

    /// <summary>
    /// Applies a force to the character's velocity
    /// </summary>
    public void AddForce(Vector2 force)
    {
        velocity += force;
    }

    /// <summary>
    /// Gets the velocity
    /// </summary>
    public Vector2 GetVelocity()
    {
        return characterController2D.velocity;
    }

    /// <summary>
    /// Checks whether or not the Character Controller is grounded
    /// </summary>
    public bool IsGrounded()
    {
        return characterController2D.isGrounded;
    }

    private void ApplyJump()
    {
        velocity.y = Mathf.Sqrt(appliedJumpPower * -Physics2D.gravity.y);
        if (appliedJumpPower >= maxJumpHeight || appliedJumpPower == oldAppliedJumpPower)
        {
            jumping = false;
        }
        else
        {
            oldAppliedJumpPower = appliedJumpPower;
        }
    }

    private void ApplyHorizontalMovement()
    {
        var smoothedMovementFactor = characterController2D.isGrounded ? groundDamping : inAirDamping;
        velocity.x = Mathf.Lerp(velocity.x, horizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
    }

    private void ApplyGravity()
    {
        velocity += Physics2D.gravity * body2D.gravityScale * Time.deltaTime;
    }

    private void ApplyVelocity()
    {
        characterController2D.move(velocity * Time.deltaTime);
        velocity = characterController2D.velocity;
    }
}