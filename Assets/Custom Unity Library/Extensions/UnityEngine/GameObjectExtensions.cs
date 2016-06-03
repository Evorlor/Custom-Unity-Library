namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// These are extensions for GameObjects
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// If GameObject has specified Component, it returns it.  If GameObject does not have specified Component, it adds it and then returns it.
        /// </summary>
        /// <typeparam name="TComponent">Type of component to add or retrieve</typeparam>
        /// <param name="gameObject">GameObject to add the component to, or retrieve it from</param>
        /// <returns>Component which was created or retrieved</returns>
        public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            TComponent component = gameObject.GetComponent<TComponent>();
            if (component == null)
            {
                component = gameObject.AddComponent(typeof(TComponent)) as TComponent;
            }
            return component;
        }

        /// <summary>
        /// Finds the nearest component of the specified type, or null if there is none.
        /// This is an expensive operation, and it should not be performed every frame.
        /// </summary>
        /// <typeparam name="TComponent">Type of component to find the nearest of</typeparam>
        /// <param name="gameObject">GameObject to use as the origin when searching for the nearest</param>
        /// <returns>The component closest to the origin GameObject</returns>
        public static TComponent FindNearest<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            TComponent nearestComponent = null;
            float nearestDistance = Mathf.Infinity;
            foreach (var component in Object.FindObjectsOfType<TComponent>())
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
}