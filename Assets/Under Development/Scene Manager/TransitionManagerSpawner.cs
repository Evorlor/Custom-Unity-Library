using UnityEngine;

/// <summary>
/// Used to predetermine spawn points for TemporarySceneManager
/// </summary>
public class TransitionManagerSpawner : MonoBehaviour
{
    [Tooltip("The list of Spawn Points in this Scene")]
    public SpawnPoint[] spawnPoints;

    void OnDrawGizmos()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.gizmo)
            {
                Gizmos.DrawIcon(spawnPoint.spawnPosition, spawnPoint.gizmo.name);
            }
        }
    }
}

[System.Serializable]
public class SpawnPoint
{
    [Tooltip("Name of the GameObject to be passed by the ScenesManager")]
    public string spawnName;

    [Tooltip("The position to spawn the GameObject")]
    public Vector2 spawnPosition;

    [Tooltip("The Gizmo used to represent the Spawn Point in the Scene")]
    public Sprite gizmo;
}