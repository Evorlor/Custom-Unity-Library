using System.Linq;
using UnityEngine;

/// <summary>
/// This is the factory for Game Objects
/// </summary>
public class GameObjectUtility
{
    private const string CloneSuffix = "(Clone)";

    /// <summary>
    /// Returns a newly created GameObject at the bottom of a tree, or returns it if it already exists.
    /// </summary>
    public static GameObject GetOrAddGameObject(params string[] gameObjectNames)
    {
        var parent = GameObject.Find(gameObjectNames[0]);
        if (!parent)
        {
            parent = new GameObject(gameObjectNames[0]);
        }
        if (gameObjectNames.Length > 1)
        {
            for (int i = 1; i < gameObjectNames.Length; i++)
            {
                GameObject child = null;
                var childTransform = parent.transform.Find(gameObjectNames[i]);
                if (childTransform)
                {
                    child = childTransform.gameObject;
                }
                else
                {
                    child = new GameObject(gameObjectNames[i]);
                    child.transform.parent = parent.transform;
                }
                parent = child;
            }
        }
        return parent;
    }

    /// <summary>
    /// Creates a GameObject by the same name of the clone and sets the clone as a child to that GameObject.  The container GameObject will automatically be destroyed once it has no remaining clones.
    /// </summary>
    public static void ChildCloneToContainer(GameObject clone)
    {
        var container = GetOrAddGameObject(clone.name.TrimEnd(CloneSuffix));
        clone.transform.parent = container.transform;
        container.GetOrAddComponent<DestroyedWhenEmpty>();
        container.hideFlags = HideFlags.HideInInspector;
    }

    /// <summary>
    /// Finds all GameObjects of the specified type, and returns them in order of ascending distance from the Vector3.
    /// This is an expensive operation, and should not be performed every frame.
    /// </summary>
    public static GameObjectType[] FindObjectsOfTypeByDistance<GameObjectType>(Vector3 position) where GameObjectType : MonoBehaviour
    {
        var gameObjectTypes = Object.FindObjectsOfType<GameObjectType>();
        gameObjectTypes = gameObjectTypes.OrderBy(o => Vector3.Distance(position, o.transform.position)).ToArray();
        return gameObjectTypes;
    }
}