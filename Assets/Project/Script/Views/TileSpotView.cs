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

        [Header("REFERENCES")]
        [SerializeField] private Button _button;
        [SerializeField] private TileParticleEffect _tileParticleEffect;
        [SerializeField] private GameObject _highlight;

        private GameObject _currentTile;

        private int _x;
        private int _y;
        private Tween _highlightTween;

        #region Unity

        private void Awake()
        {
            _button.onClick.AddListener(OnTileClick);
            _highlight.SetActive(false);
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
            _currentTile = tile;
            tile.transform.SetParent(transform, false);
            tile.transform.position = transform.position;
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

        public void PlayDestroyParticles_ColorBomb() => _tileParticleEffect.PlayColorBombEffect();

        public void SetHintHighlight(bool newState)
        {
            if (_highlight == null) return;

            _highlight.SetActive(newState);
            _highlight.transform.localScale = Vector3.one;

            if (_highlightTween != null)
            {
                _highlightTween?.Kill();
            }

            if (newState)
            {
                _highlightTween = _highlight.transform.DOScale(1.1f, 0.4f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}
