using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Utility methods for Scriptable Objects
/// </summary>
public static class ScriptableObjectUtility
{
    private const string AssetsFolderName = "Assets";
    private const string AssetSuffix = ".asset";

    /// <summary>
    //	Create, name, and place unique new ScriptableObject asset files, optionally passing in the path where the Scriptable Object is to be created.
    /// </summary>
    public static TAsset CreateAsset<TAsset>(string path = null) where TAsset : ScriptableObject
    {
        TAsset asset = ScriptableObject.CreateInstance<TAsset>();
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (assetPath == "")
        {
            assetPath = AssetsFolderName;
        }
        else if (Path.GetExtension(assetPath) != "")
        {
            assetPath = assetPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        if (path == null || path == "")
        {
            path = assetPath + "/" + typeof(TAsset).ToString() + AssetSuffix;
        }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path);
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        return asset;
    }
}