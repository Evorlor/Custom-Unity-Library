using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    private const string AssetsFolderName = "Assets";
    private const string AssetSuffix = ".asset";

    /// <summary>
    //	Create, name, and place unique new ScriptableObject asset files, optionally passing in the path where the Scriptable Object is to be created.
    /// </summary>
    public static AssetType CreateAsset<AssetType>(string path = null) where AssetType : ScriptableObject
    {
        AssetType asset = ScriptableObject.CreateInstance<AssetType>();
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
            path = assetPath + "/" + typeof(AssetType).ToString() + AssetSuffix;
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