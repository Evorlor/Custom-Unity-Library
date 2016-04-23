using UnityEngine;
using System.Collections;

public class DestroyedWhenEmpty : MonoBehaviour
{
    void Update()
    {
        bool hasExtraComponents = gameObject.GetComponents<MonoBehaviour>().Length > 1;
        bool hasChildren = transform.childCount > 0;
        if (!hasExtraComponents && !hasChildren)
        {
            Destroy(gameObject);
        }
    }
}