namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// Used to predetermine spawn points for TemporarySceneManager
    /// </summary>
    public class TransitionManagerSpawner : MonoBehaviour
    {
        [Tooltip("The list of Spawn Points in this Scene")]
        [SerializeField]
        private SpawnPoint[] spawnPoints;

        void OnDrawGizmos()
        {
            foreach (var spawnPoint in spawnPoints)
            {
                if (spawnPoint.GetGizmo())
                {
                    Gizmos.DrawIcon(spawnPoint.GetSpawnPosition(), spawnPoint.GetGizmo().name);
                }
            }
        }

        public SpawnPoint[] GetSpawnPoints()
        {
            return spawnPoints;
        }
    }

    [System.Serializable]
    public class SpawnPoint
    {
        [Tooltip("Name of the GameObject to be passed by the ScenesManager")]
        [SerializeField]
        private string spawnName;

        [Tooltip("The position to spawn the GameObject")]
        [SerializeField]
        private Vector2 spawnPosition;

        [Tooltip("The Gizmo used to represent the Spawn Point in the Scene")]
        [SerializeField]
        private Sprite gizmo;

        public string GetSpawnName()
        {
            return spawnName;
        }

        public Vector2 GetSpawnPosition()
        {
            return spawnPosition;
        }

        public Sprite GetGizmo()
        {
            return gizmo;
        }
    }
}