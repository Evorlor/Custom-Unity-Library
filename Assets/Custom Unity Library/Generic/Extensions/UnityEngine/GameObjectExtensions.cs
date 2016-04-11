using UnityEngine;

/// <summary>
/// These are extensions for GameObjects
/// </summary>
public static class GameObjectExtensions
{
    /// <summary>
    /// If GameObject has specified Component, it returns it.  If GameObject does not have specified Component, it adds it and then returns it.
    /// </summary>
    public static ComponentType GetOrAddComponent<ComponentType>(this GameObject gameObject) where ComponentType : Component
    {
        ComponentType component = gameObject.GetComponent<ComponentType>();
        if (component == null)
        {
            component = gameObject.AddComponent(typeof(ComponentType)) as ComponentType;
        }
        return component;
    }

    /// <summary>
    /// Finds the nearest component of the specified type, or null if there is none.
    /// This is an expensive operation, and it should not be performed every frame.
    /// </summary>
    public static ComponentType FindNearest<ComponentType>(this GameObject gameObject) where ComponentType : Component
    {
        ComponentType nearestComponent = null;
        float nearestDistance = Mathf.Infinity;
        foreach (var component in Object.FindObjectsOfType<ComponentType>())
        {
            if (component.gameObject == gameObject)
            {
                continue;
            }
            float currentDistance = Vector3.Distance(gameObject.transform.position, component.transform.position);
            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestComponent = component;
            }
        }
        return nearestComponent;
    }
}