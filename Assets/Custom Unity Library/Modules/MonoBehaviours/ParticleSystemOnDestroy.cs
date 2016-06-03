namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// Plays the provided Particle System when the GameObject is destroyed
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class ParticleSystemOnDestroy : MonoBehaviour
    {
        [Tooltip("The particle system which will be trigged when game object is destroyed")]
        [SerializeField]
        private ParticleSystem ParticleSystem;

        private bool applicationClosing;

        void OnDestroy()
        {
            if (applicationClosing)
            {
                return;
            }
            if (GetComponent<Renderer>().isVisible)
            {
                var particleSystem = Instantiate(ParticleSystem, transform.position, Quaternion.identity) as ParticleSystem;
                GameObjectUtility.ChildCloneToContainer(particleSystem.gameObject);
                Destroy(particleSystem.gameObject, particleSystem.duration);
            }
        }

        void OnApplicationQuit()
        {
            applicationClosing = true;
        }
    }
}