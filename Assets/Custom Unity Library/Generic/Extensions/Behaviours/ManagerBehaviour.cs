using UnityEngine;

/// <summary>
/// Extending this class creates a MonoBehaviour which may only have on instance and will not be destroyed between scenes.  When extending, the type of the inheriting class must be passed.
/// </summary>
public abstract class ManagerBehaviour<ManagerType> : MonoBehaviour where ManagerType : ManagerBehaviour<ManagerType>
{
    private const string ManagerName = "Manager";

    private static ManagerType instance;

    /// <summary>
    /// Gets the singleton instance of the Manager
    /// </summary>
    public static ManagerType Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<ManagerType>();
                if (!instance)
                {
                    var masterManager = GameObjectUtility.GetOrAddGameObject(ManagerName);
                    var manager = GameObjectUtility.GetOrAddGameObject(typeof(ManagerType).ToString());
                    manager.transform.SetParent(masterManager.transform);
                    instance = manager.AddComponent<ManagerType>();
                }
                var root = instance.transform;
                while (root.parent)
                {
                    root = root.parent;
                }
                DontDestroyOnLoad(root.gameObject);
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        DestroyDuplicateManagers();
    }

    private void DestroyDuplicateManagers()
    {
        var managers = FindObjectsOfType<ManagerType>();
        foreach (var manager in managers)
        {
            if (!manager)
            {
                continue;
            }
            if (Instance != manager)
            {
                bool sharesGameObjectWithManager = Instance.gameObject == manager.gameObject;
                bool hasExtraComponents = manager.GetComponents<MonoBehaviour>().Length > 1;
                bool hasChildren = transform.childCount > 0;
                bool destroyGameObject = !sharesGameObjectWithManager && !hasExtraComponents && !hasChildren;
                if (destroyGameObject)
                {
                    Destroy(manager.gameObject);
                }
                else
                {
                    Destroy(manager);
                }
            }
        }
    }
}