using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Extending this class creates a MonoBehaviour which may only have on instance and will not be destroyed between scenes.  When extending, the type of the inheriting class must be passed.
/// </summary>
public abstract class ManagerBehaviour<TManager> : MonoBehaviour where TManager : ManagerBehaviour<TManager>
{
    private const string ManagerName = "Manager";

    private static TManager instance;

    /// <summary>
    /// Gets the singleton instance of the Manager
    /// </summary>
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
                    var managerName = Regex.Replace(typeof(TManager).ToString(), @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
                    var manager = GameObjectUtility.GetOrAddGameObject(managerName);
                    manager.transform.SetParent(masterManager.transform);
                    instance = manager.AddComponent<TManager>();
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
        var managers = FindObjectsOfType<TManager>();
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