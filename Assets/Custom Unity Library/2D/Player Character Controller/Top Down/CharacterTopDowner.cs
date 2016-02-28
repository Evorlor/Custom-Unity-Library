using UnityEngine;
using Prime31;
using System;

/// <summary>
/// This is an extension to Prime31's CharacterController2D for top down controls
/// </summary>
[RequireComponent(typeof(CharacterController2D))]
public class CharacterTopDowner : MonoBehaviour
{
    [Tooltip("Speed at which the character can move")]
    [SerializeField]
    [Range(0, 25)]
    private float moveSpeed = 8.0f;

    [Tooltip("How fast the character can change directions")]
    [SerializeField]
    [Range(0, 100)]
    private float damping = 20.0f;

    public event Action<RaycastHit2D> onControllerCollidedEvent;
    public event Action<Collider2D> onTriggerEnterEvent;
    public event Action<Collider2D> onTriggerStayEvent;
    public event Action<Collider2D> onTriggerExitEvent;

    private float horizontalSpeed;
    private float verticalSpeed;
    private CharacterController2D characterController2D;
    private RaycastHit2D lastControllerColliderHit;
    private Vector2 velocity;
    private Rigidbody2D body2D;

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
        ApplyMovement();
        ApplyRotation();
        ApplyGravity();
        ApplyVelocity();
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    public void Move(Vector2 movement)
    {
        horizontalSpeed = movement.x;
        verticalSpeed = movement.y;
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    public void Move(float horizontalMovement, float verticalMovement)
    {
        horizontalSpeed = horizontalMovement;
        verticalSpeed = verticalMovement;
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

    private void ApplyMovement()
    {
        velocity.x = Mathf.Lerp(velocity.x, horizontalSpeed * moveSpeed, Time.deltaTime * damping);
        velocity.y = Mathf.Lerp(velocity.y, verticalSpeed * moveSpeed, Time.deltaTime * damping);
    }

    private void ApplyRotation()
    {
        if (Mathf.Abs(velocity.x) > 0 || Mathf.Abs(velocity.y) > 0)
        {
            float rotation = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
        }
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