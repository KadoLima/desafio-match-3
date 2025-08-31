using UnityEngine;

namespace Gazeus.DesafioMatch3.Effects
{
    public class TileParticleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private ParticleSystem.MainModule _mainModule;

        #region Unity
        private void Awake()
        {
            _mainModule = _particleSystem.main;
        }
        #endregion

        public void PlayDestroyParticles(Color color)
        {
            _mainModule.startColor = color;
            _particleSystem.Play();
        }
    }
}
