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
}