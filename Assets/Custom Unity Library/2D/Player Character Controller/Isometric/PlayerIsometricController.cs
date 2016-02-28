using UnityEngine;

/// <summary>
/// This is an example controller for using the CharacterIsometric
/// </summary>
[RequireComponent(typeof(CharacterIsometric))]
public class PlayerIsometricController : MonoBehaviour
{
    [Tooltip("Speed at which the player moves vertically relative to the horizontal speed.")]
    [SerializeField]
    [Range(0, 2)]
    private float verticalSpeedMultiplier = 0.5f;

    private CharacterIsometric characterIsometric;

    void Awake()
    {
        characterIsometric = GetComponent<CharacterIsometric>();
        characterIsometric.onControllerCollidedEvent += onControllerCollider;
        characterIsometric.onTriggerEnterEvent += onTriggerEnterEvent;
        characterIsometric.onTriggerStayEvent += onTriggerStayEvent;
        characterIsometric.onTriggerExitEvent += onTriggerExitEvent;
    }

    void Update()
    {
        characterIsometric.Move(Input.GetAxis(InputNames.Horizontal), Input.GetAxis(InputNames.Vertical) * verticalSpeedMultiplier);
    }

    void onControllerCollider(RaycastHit2D hit)
    {
        //Debug.Log("onControllerCollider: " + hit.transform.gameObject.name);
    }


    void onTriggerEnterEvent(Collider2D collider2D)
    {
        //Debug.Log("onTriggerEnterEvent: " + collider2D.gameObject.name);
    }

    void onTriggerStayEvent(Collider2D collider2D)
    {
        //Debug.Log("onTriggerStayEvent: " + collider2D.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D collider2D)
    {
        //Debug.Log("onTriggerExitEvent: " + collider2D.gameObject.name);
    }
}