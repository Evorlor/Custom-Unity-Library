using UnityEngine;

/// <summary>
/// Projectiles are specified components with a Rigidbody2D
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D body2D;

    void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Gets the Rigidbody2D for the Projectile
    /// </summary>
    /// <returns>The Projectile's Rigidbody2D</returns>
    public Rigidbody2D GetRigidBody2D()
    {
        return body2D;
    }
}