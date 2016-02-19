using UnityEngine;

/// <summary>
/// This is an example controller for using the CharacterTopDowner
/// </summary>
[RequireComponent(typeof(CharacterTopDowner))]
public class PlayerController : MonoBehaviour
{
    private CharacterTopDowner characterMoverTopDown;

    void Awake()
    {
        characterMoverTopDown = GetComponent<CharacterTopDowner>();
        characterMoverTopDown.onControllerCollidedEvent += onControllerCollider;
        characterMoverTopDown.onTriggerEnterEvent += onTriggerEnterEvent;
        characterMoverTopDown.onTriggerStayEvent += onTriggerStayEvent;
        characterMoverTopDown.onTriggerExitEvent += onTriggerExitEvent;
    }

    void Update()
    {
        characterMoverTopDown.Move(Input.GetAxis(InputNames.Horizontal), Input.GetAxis(InputNames.Vertical));
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