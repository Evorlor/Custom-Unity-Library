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
                }
                return instance;
            }
        }

        /// <summary>
        /// Confirms or corrects singleton status.
        /// </summary>
        protected virtual void Awake()
        {
            if (!instance)
            {
                instance = this as TManager;
            }
            SetUpDestruction(transform);
            DestroyDuplicateManagers();
        }

        /// <summary>
        /// Updates singleton constraints when level is loaded
        /// </summary>
        protected virtual void OnLevelWasLoaded()
        {
            SetUpDestruction(transform);
        }

        private void SetUpDestruction(Transform leaf)
        {
            do
            {
                var destroyedWhenEmpty = leaf.gameObject.GetComponent<DestroyedWhenEmpty>();
                if (!destroyedWhenEmpty)
                {
                    destroyedWhenEmpty = leaf.gameObject.AddComponent<DestroyedWhenEmpty>();
                    destroyedWhenEmpty.hideFlags = HideFlags.HideInInspector;
                }
                if (!leaf.parent)
                {
                    DontDestroyOnLoad(leaf);
                }
                leaf = leaf.parent;
            } while (leaf);
        }

        private void DestroyDuplicateManagers()
        {
            var managers = FindObjectsOfType<TManager>();
            foreach (var manager in managers)
            {
                if (Instance != manager)
                {
                    SetUpDestruction(manager.transform);
                    Destroy(manager);
                }
            }
        }
    }
}