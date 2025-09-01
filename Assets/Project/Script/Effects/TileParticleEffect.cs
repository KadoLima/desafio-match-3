using UnityEngine;

namespace Gazeus.DesafioMatch3.Effects
{
    public class TileParticleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _defaultParticles;
        [SerializeField] private ParticleSystem _colorBombParticles;

        private ParticleSystem.MainModule _mainModule;

        #region Unity
        private void Awake()
        {
            _mainModule = _defaultParticles.main;
        }
        #endregion

        public void PlayDestroyParticles(Color color)
        {
            _mainModule.startColor = color;
            _defaultParticles.Play();
        }

        internal void PlayColorBombEffect()
        {
            _colorBombParticles.Play();
        }
    }
}
