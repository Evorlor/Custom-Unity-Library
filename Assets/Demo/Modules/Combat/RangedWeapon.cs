using CustomUnityLibrary;
using UnityEngine;

/// <summary>
/// Ranged Weapons can be used to spawn and fire Projectiles
/// </summary>
[ExecuteInEditMode]
public class RangedWeapon : MonoBehaviour
{
    private const string SpawnPositionGizmoName = "gizmo_ranged_weapon_spawn_position.png";

    [Tooltip("Projectile shot by this Ranged Weapon")]
    [SerializeField]
    private Projectile projectile;

    [Tooltip("Speed at which the Projectile fires.  This will be adjusted based on the arch used.")]
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float force = 10.0f;

    [Tooltip("Rate at which the RangedWeapon can fire")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float fireCooldown = 1.0f;

    [Tooltip("Whether or not this RangedWeapon will fire even if the target is out of its range")]
    [SerializeField]
    private bool unlimitedRange = false;

    [Tooltip("How far this Ranged Weapon can fire, assuming it does not have unlimited range.  It will not fire if its target is beyond this distance.")]
    [SerializeField]
    [Range(1.0f, 1000.0f)]
    private float range = 100.0f;

    [Tooltip("Arch to be used when firing.  An arch of 0 or 1 will fire at the full force.")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float arch = 0.5f;

    [Tooltip("Position where the projectiles will spawn")]
    [SerializeField]
    private Vector3 projectileSpawnOffset;

    private float totalCooldown;
    private Vector3 oldRightVector;

    void Awake()
    {
        totalCooldown = fireCooldown;
        oldRightVector = transform.right;
    }

    void Update()
    {
        UpdateProjectileSpawnPosition();
        UpdateFireCooldown();
    }

    void OnValidate()
    {
        if (unlimitedRange)
        {
            range = Mathf.Infinity;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(transform.position + projectileSpawnOffset, SpawnPositionGizmoName);
    }

    /// <summary>
    /// Fires the equipped Projectile towards the Ranged Weapon's Right Vector.
    /// </summary>
    /// <returns>Whether or not the Projectile was successfully fired</returns>
    public bool Fire()
    {
        return Fire(transform.right);
    }

    /// <summary>
    /// Fires the equipped Projectile at the target.
    /// </summary>
    /// <param name="target">Target to fire the projectile at</param>
    /// <param name="ignoreGravity">Whether or not to ignore gravity while firing the projectile</param>
    /// <returns>Whether or not the projectile was successfully fired</returns>
    public bool Fire(Transform target, bool ignoreGravity = false)
    {
        if (ignoreGravity)
        {
            var direction = target.position - transform.position + projectileSpawnOffset;
            return Fire(direction, ignoreGravity);
        }
        if (!IsValidShot())
        {
            return false;
        }
        var projectileInstance = Instantiate(projectile, transform.position + projectileSpawnOffset, Quaternion.identity) as Projectile;
        GameObjectUtility.ChildCloneToContainer(projectileInstance.gameObject);
        var projectileBody = projectileInstance.GetRigidBody2D();
        if (!unlimitedRange && !projectileBody.IsWithinRange(target.position, force, arch))
        {
            Destroy(projectileInstance.gameObject);
            return false;
        }
        projectileBody.SetTrajectory(target.position, force, arch);
        DisableProjectileColliders(projectileInstance);
        fireCooldown = totalCooldown;
        return true;
    }

    /// <summary>
    /// Fires the equipped Projectile in the direction
    /// </summary>
    /// <param name="direction">The direction to fire the Projectile in</param>
    /// <param name="ignoreGravity">Whether or not to ignore gravity when firing the Projectile</param>
    /// <returns></returns>
    public bool Fire(Vector2 direction, bool ignoreGravity = false)
    {
        if (!IsValidShot(direction))
        {
            return false;
        }
        direction.Normalize();
        var projectileInstance = Instantiate(projectile, transform.position + projectileSpawnOffset, Quaternion.identity) as Projectile;
        GameObjectUtility.ChildCloneToContainer(projectileInstance.gameObject);
        var projectileBody = projectileInstance.GetRigidBody2D();
        if (ignoreGravity)
        {
            projectileBody.gravityScale = 0;
        }
        if (projectileBody.gravityScale != 0)
        {
            projectileBody.AddForce(direction * force, ForceMode2D.Impulse);
        }
        else
        {
            projectileBody.velocity = direction * force;
        }
        DisableProjectileColliders(projectileInstance);
        fireCooldown = totalCooldown;
        return true;
    }

    /// <summary>
    /// Makes the Projectiles not check for collision with the Ranged Weapon
    /// </summary>
    /// <param name="projectileInstance">Projectile whose collision will be ignored</param>
    private void DisableProjectileColliders(Projectile projectileInstance)
    {
        var rangedWeaponColliders = GetComponents<Collider2D>();
        var projectileColliders = projectileInstance.GetComponents<Collider2D>();
        if (rangedWeaponColliders.Length > 0 && projectileColliders.Length > 0)
        {
            foreach (var rangedWeaponCollider in rangedWeaponColliders)
            {
                foreach (var projectileCollider in projectileColliders)
                {
                    Physics2D.IgnoreCollision(rangedWeaponCollider, projectileCollider);
                }
            }
        }
    }

    /// <summary>
    /// Moves the Projectile to the barrel of the Ranged Weapon
    /// </summary>
    private void UpdateProjectileSpawnPosition()
    {
        if (transform.right == oldRightVector)
        {
            return;
        }
        bool clockwiseRotation = Vector3.Cross(transform.right, oldRightVector).z > 0;
        var angleToRotate = Vector2.Angle(oldRightVector, transform.right);
        if (clockwiseRotation)
        {
            angleToRotate *= -1;
        }
        var updatedDirection = (Quaternion.AngleAxis(angleToRotate, Vector3.forward) * projectileSpawnOffset).normalized;
        projectileSpawnOffset = updatedDirection * projectileSpawnOffset.magnitude;
        oldRightVector = transform.right;
    }

    /// <summary>
    /// Checks to make sure the shot is valid
    /// </summary>
    /// <returns>Whether or not the shot is valid</returns>
    private bool IsValidShot()
    {
        if (fireCooldown > 0)
        {
            return false;
        }
        if (!projectile)
        {
            Debug.LogWarning(name + " is missing a Projectile.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks to make sure the shot power required is within the capabilities of this Ranged Weapon
    /// </summary>
    /// <param name="direction">The direction whose magnitude will be used</param>
    /// <returns>Whether or not the shot is within range</returns>
    private bool IsValidShot(Vector2 direction)
    {
        if (!unlimitedRange && direction.magnitude > range)
        {
            return false;
        }
        return IsValidShot();
    }

    /// <summary>
    /// Updates the cooldown rate of the RangedWeapon
    /// </summary>
    private void UpdateFireCooldown()
    {
        fireCooldown -= Time.deltaTime;
        fireCooldown = Mathf.Max(0, fireCooldown);
    }
}