﻿using UnityEngine;

/// <summary>
/// Destroys a GameObject when it has no children nor components
/// </summary>
[DisallowMultipleComponent]
public class DestroyedWhenEmpty : MonoBehaviour
{
    void Update()
    {
        if (IsEmpty())
        {
            Destroy(gameObject);
        }
    }

    private bool IsEmpty()
    {
        bool hasExtraComponents = transform.GetComponents<MonoBehaviour>().Length > 1;
        bool hasChildren = transform.childCount > 0;
        return !hasExtraComponents && !hasChildren;
    }
}