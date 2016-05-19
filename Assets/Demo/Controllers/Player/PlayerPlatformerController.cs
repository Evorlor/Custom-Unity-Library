using CustomUnityLibrary;
using UnityEngine;

/// <summary>
/// This is an example controller for using the CharacterPlatformer
/// </summary>
[RequireComponent(typeof(CharacterPlatformer))]
public class PlayerPlatformerController : MonoBehaviour
{
    private const float dropDownForceRequired = 0.5f;

    private CharacterPlatformer characterPlatformer;

    void Awake()
    {
        characterPlatformer = GetComponent<CharacterPlatformer>();
        characterPlatformer.onControllerCollidedEvent += onControllerCollider;
        characterPlatformer.onTriggerEnterEvent += onTriggerEnterEvent;
        characterPlatformer.onTriggerStayEvent += onTriggerStayEvent;
        characterPlatformer.onTriggerExitEvent += onTriggerExitEvent;
    }

    void Update()
    {
        characterPlatformer.Move(Input.GetAxis(InputNames.Horizontal));
        if (Input.GetButton(InputNames.Jump))
        {
            if (Input.GetAxis(InputNames.Vertical) <= -dropDownForceRequired)
            {
                characterPlatformer.DropDownPlatform();
            }
            else
            {
                characterPlatformer.Jump();
            }
        }
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