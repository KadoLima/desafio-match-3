using System;
using DG.Tweening;
using Gazeus.DesafioMatch3.Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.Views
{
    public class TileSpotView : MonoBehaviour
    {
        public event Action<int, int> Clicked;

        [SerializeField] private Button _button;
        [SerializeField] private TileParticleEffect _tileParticleEffect;
        private GameObject _currentTile;

        private int _x;
        private int _y;

        #region Unity

        private void Awake()
        {
            _button.onClick.AddListener(OnTileClick);
        }

        #endregion

        public Tween AnimatedSetTile(GameObject tile)
        {
            tile.transform.SetParent(transform);
            tile.transform.DOKill();
            _currentTile = tile;

            return tile.transform.DOMove(transform.position, 0.3f);
        }

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public void SetTile(GameObject tile)
        {
            tile.transform.SetParent(transform, false);
            tile.transform.position = transform.position;
            _currentTile = tile;
        }

        private void OnTileClick()
        {
            Clicked?.Invoke(_x, _y);
        }

        public void PlayDestroyParticles_Default()
        {
            Image currentTileImage = _currentTile.GetComponent<Image>();
            _tileParticleEffect.PlayDestroyParticles(currentTileImage.color);
        }

        public void PlayDestroyParticles_ColorBomb()
        {
            _tileParticleEffect.PlayColorBombEffect();
        }
    }
}
