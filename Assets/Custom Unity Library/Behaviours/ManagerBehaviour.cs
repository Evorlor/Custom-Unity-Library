namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// Extending this class creates a MonoBehaviour which may only have on instance and will not be destroyed between scenes.  When extending, the type of the inheriting class must be passed.
    /// </summary>
    /// <typeparam name="TManager">The type of manager to be created.</typeparam>
    [DisallowMultipleComponent]
    public abstract class ManagerBehaviour<TManager> : MonoBehaviour where TManager : ManagerBehaviour<TManager>
    {
        private const string ManagerName = "Manager";

        private static TManager instance;

        /// <summary>
        /// Gets the singleton instance of the Manager
        /// </summary>
        /// <value>The singleton instance of the Manager Type</value>
        public static TManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType<TManager>();
                    if (!instance)
                    {
                        var masterManager = GameObjectUtility.GetOrAddGameObject(ManagerName);
                        var managerName = typeof(TManager).ToString().AddSpacing();
                        var manager = GameObjectUtility.GetOrAddGameObject(masterManager.name, managerName);
                        instance = manager.AddComponent<TManager>();
                    }
                    SetUpDestruction(instance.transform);
                }
                return instance;
            }
        }

        private static void SetUpDestruction(Transform leaf)
        {
            do
            {
                var destroyedWhenEmpty = leaf.gameObject.GetOrAddComponent<DestroyedWhenEmpty>();
                destroyedWhenEmpty.hideFlags = HideFlags.HideInInspector;
                if (!leaf.parent)
                {
                    DontDestroyOnLoad(leaf.gameObject);
                }
                leaf = leaf.parent;
            } while (leaf);
        }
    }
}