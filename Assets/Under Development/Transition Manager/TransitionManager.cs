namespace CustomUnityLibrary
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// This is used to load a scene for temporary use
    /// </summary>
    public class TransitionManager : ManagerBehaviour<TransitionManager>
    {
        public delegate void TemporarySceneLoaded(Scene temporaryScene);
        public event TemporarySceneLoaded TemporarySceneLoader;

        /// <summary>
        /// This is the data that can be saved in the temporary scene and returned to the permanent scene.
        /// </summary>
        public object savedData = new object();

        private bool temporarySceneActive = false;
        private Scene permanentActiveScene;
        private Scene temporaryActiveScene;
        private Dictionary<GameObject, bool> permanentGameObjects = new Dictionary<GameObject, bool>();
        private Dictionary<GameObject, PersistentGameObjectData> temporaryGameObjects = new Dictionary<GameObject, PersistentGameObjectData>();
        private bool copying;
        private bool transitioning;
        private bool midScene;
        private float transitionAlpha = 0.0f;
        private float transitionTime;
        private float oldTimeScale = 1.0f;

        private const string MainCameraTag = "MainCamera";

        void OnApplicationQuit()
        {
            SceneManager.UnloadScene(temporaryActiveScene.name);
        }

        void OnGUI()
        {
            if (transitioning || midScene)
            {
                var texture = new Texture2D(Screen.width, Screen.height);
                var overlay = new Rect(0, 0, Screen.width, Screen.height);
                var color = new Color(0, 0, 0, midScene ? 1.0f : transitionAlpha);
                GUI.color = color;
                GUI.Box(overlay, texture);
            }
        }

        /// <summary>
        /// Temporarily loads a Scene given the Scene build index until UnloadTemporaryScene() is called, at which point the original Scene will become active again.
        /// </summary>
        public void LoadTemporaryScene(int sceneBuildIndex, float transitionTime = 0.5f)
        {
            LoadTemporaryScene(sceneBuildIndex, false);
        }

        /// <summary>
        /// Temporarily loads a Scene given the Scene name until UnloadTemporaryScene() is called, at which point the original Scene will become active again.
        /// </summary>
        public void LoadTemporaryScene(string sceneName, float transitionTime = 0.5f)
        {
            LoadTemporaryScene(sceneName, false);
        }

        /// <summary>
        /// Temporarily loads a Scene given the Scene build index until UnloadTemporaryScene() is called, at which point the original Scene will become active again.  GameObjects can be passed into the temporary Scene.  Either copies or the existing instances of GameObjects may be passed.  The GameObjects Transforms will be preserved.
        /// </summary>
        public void LoadTemporaryScene(int sceneBuildIndex, bool copy, float transitionTime = 0.5f, params GameObject[] gameObjects)
        {
            LoadTemporaryScene(SceneManager.GetSceneAt(sceneBuildIndex).name, copy, transitionTime, gameObjects);
        }

        /// <summary>
        /// Temporarily loads a Scene given the Scene name until UnloadTemporaryScene() is called, at which point the original Scene will become active again.  GameObjects can be passed into the temporary Scene.  Either copies or the existing instances of GameObjects may be passed.  The GameObjects Transforms will be preserved.
        /// </summary>
        public void LoadTemporaryScene(string sceneName, bool copy, float transitionTime = 0.5f, params GameObject[] gameObjects)
        {
            if (temporarySceneActive)
            {
                Debug.LogWarning("Attempting to load temporary Scene " + sceneName + ", but temporary Scene " + temporaryActiveScene.name + " is already active.  Temporary load has failed.");
                return;
            }
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            this.transitionTime = transitionTime;
            StartCoroutine(LoadSceneAfterTransition(sceneName, copy, transitionTime, gameObjects));
        }

        /// <summary>
        /// Unloads the temporary Scene and returns to the permanent Scene.  Any data stored in the field savedData will remain accessible until the next temporary Scene is loaded.
        /// </summary>
        public void UnloadTemporaryScene()
        {
            if (!temporarySceneActive)
            {
                Debug.LogWarning("There is no temporary Scene active.  Temporary unload has failed.");
                return;
            }
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            StartCoroutine(UnloadSceneAfterTransition());
        }

        private IEnumerator MakeTransition()
        {
            transitioning = true;
            bool fading = transitionAlpha == 0.0f;
            while (transitioning)
            {
                transitionAlpha += (fading ? Time.unscaledDeltaTime : -Time.unscaledDeltaTime) / transitionTime;
                transitionAlpha = Mathf.Clamp01(transitionAlpha);
                yield return new WaitForEndOfFrame();
                if (transitionAlpha == 0 || transitionAlpha == 1)
                {
                    transitioning = false;
                }
            }
        }

        private IEnumerator LoadSceneAfterTransition(string sceneName, bool copy, float transitionTime = 0.5f, params GameObject[] gameObjects)
        {
            StartCoroutine(MakeTransition());
            savedData = new object();
            copying = copy;
            temporarySceneActive = true;
            permanentActiveScene = SceneManager.GetActiveScene();
            permanentGameObjects = new Dictionary<GameObject, bool>();
            temporaryGameObjects = new Dictionary<GameObject, PersistentGameObjectData>();
            foreach (var gameObjectToPass in gameObjects)
            {
                temporaryGameObjects.Add(copy ? Instantiate(gameObjectToPass) : gameObjectToPass, new PersistentGameObjectData(gameObjectToPass.activeSelf, gameObjectToPass.transform, gameObjectToPass.transform.parent));
            }
            yield return new WaitWhile(() => transitioning);
            midScene = true;
            foreach (var permanentGameObject in permanentActiveScene.GetRootGameObjects())
            {
                if (permanentGameObject.activeSelf)
                {
                    if (permanentGameObject.tag != MainCameraTag)
                    {
                        permanentGameObjects.Add(permanentGameObject, false);
                        permanentGameObject.SetActive(false);
                    }
                    else
                    {
                        var audioListener = permanentGameObject.GetComponent<AudioListener>();
                        if (audioListener && audioListener.enabled)
                        {
                            permanentGameObjects.Add(permanentGameObject, true);
                            audioListener.enabled = false;
                        }
                    }
                }
            }
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            temporaryActiveScene = SceneManager.GetSceneByName(sceneName);
            yield return new WaitUntil(() => temporaryActiveScene.isLoaded);
            foreach (var temporaryGameObject in temporaryGameObjects)
            {
                temporaryGameObject.Key.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(temporaryGameObject.Key, temporaryActiveScene);
                temporaryGameObject.Key.SetActive(temporaryGameObject.Value.Active);
            }
            SceneManager.SetActiveScene(temporaryActiveScene);
            Time.timeScale = 0.0f;
            if (TemporarySceneLoader != null)
            {
                TemporarySceneLoader(temporaryActiveScene);
            }
            MovePrespawnedGameObjects();
            midScene = false;
            StartCoroutine(MakeTransition());
            yield return new WaitWhile(() => transitioning);
            Time.timeScale = oldTimeScale;
        }

        private IEnumerator UnloadSceneAfterTransition()
        {
            StartCoroutine(MakeTransition());
            var audioListeners = FindObjectsOfType<AudioListener>();
            foreach (var audioListener in audioListeners)
            {
                audioListener.enabled = false;
            }
            yield return new WaitWhile(() => transitioning);
            midScene = true;
            foreach (var permanentGameObject in permanentGameObjects)
            {
                if (!permanentGameObject.Value)
                {
                    permanentGameObject.Key.SetActive(true);
                }
                else
                {
                    var audioListener = permanentGameObject.Key.GetComponent<AudioListener>();
                    if (audioListener)
                    {
                        audioListener.enabled = true;
                    }
                }
            }
            if (!copying)
            {
                foreach (var temporaryGameObject in temporaryGameObjects)
                {
                    if (!temporaryGameObject.Key)
                    {
                        continue;
                    }
                    temporaryGameObject.Key.transform.position = temporaryGameObject.Value.Position;
                    temporaryGameObject.Key.transform.rotation = temporaryGameObject.Value.Rotation;
                    temporaryGameObject.Key.transform.localScale = temporaryGameObject.Value.Scale;
                    SceneManager.MoveGameObjectToScene(temporaryGameObject.Key, permanentActiveScene);
                    temporaryGameObject.Key.transform.SetParent(temporaryGameObject.Value.Parent);
                }
            }
            SceneManager.SetActiveScene(permanentActiveScene);
            yield return new WaitForEndOfFrame();
            SceneManager.UnloadScene(temporaryActiveScene.name);
            midScene = false;
            StartCoroutine(MakeTransition());
            temporarySceneActive = false;
            yield return new WaitWhile(() => transitioning);
            Time.timeScale = oldTimeScale;
        }

        private void MovePrespawnedGameObjects()
        {
            var spawner = FindObjectOfType<TransitionManagerSpawner>();
            if (spawner)
            {
                foreach (var spawnPoint in spawner.GetSpawnPoints())
                {
                    var spawnedGameObject = GameObject.Find(spawnPoint.GetSpawnName());
                    if (!spawnedGameObject)
                    {
                        if (spawnPoint.GetSpawnName() == string.Empty)
                        {
                            Debug.LogWarning("No name for the GameObject expected at the spawn point located at " + spawnPoint.GetSpawnPosition() + " was specified, and that spawn point will have no effect.");
                        }
                        else
                        {
                            Debug.LogWarning(spawnPoint.GetSpawnName() + " was not sent to " + temporaryActiveScene.name + " from " + permanentActiveScene.name + ", and its spawn position will not be updated.", spawner.gameObject);
                        }
                        continue;
                    }
                    spawnedGameObject.transform.position = spawnPoint.GetSpawnPosition();
                }
            }
        }

        private class PersistentGameObjectData
        {
            /// <summary>
            /// Creates data to store whether or not a GameObject is active and its Transform
            /// </summary>
            public PersistentGameObjectData(bool active, Transform transform, Transform parent)
            {
                Active = active;
                Position = transform.position;
                Rotation = transform.rotation;
                Scale = transform.localScale;
                Parent = parent;
            }

            /// <summary>
            /// Whether or not the GameObject is active
            /// </summary>
            public bool Active { get; set; }

            /// <summary>
            /// The GameObject's position
            /// </summary>
            public Vector3 Position { get; set; }

            /// <summary>
            /// The GameObject's rotation
            /// </summary>
            public Quaternion Rotation { get; set; }

            /// <summary>
            /// The GameObject's scale
            /// </summary>
            public Vector3 Scale { get; set; }

            /// <summary>
            /// The GameObject's parent
            /// </summary>
            public Transform Parent { get; set; }
        }
    }
}