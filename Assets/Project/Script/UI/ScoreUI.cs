using DG.Tweening;
using Gazeus.DesafioMatch3.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Gazeus.DesafioMatch3.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private PlayerCurrencySO _currencyToTrack;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("SCORE TEXT ANIMATION PARAMETERS")]
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _scaleAmount = 1.2f;
        [SerializeField] private float _scaleDuration = 0.1f;

        private int _currentValue = 0;
        private Tween _countTween;

        #region Unity
        private void OnEnable()
        {
            _currencyToTrack.OnCurrencyChanged += AnimateScoreText;
        }

        private void OnDisable()
        {
            _currencyToTrack.OnCurrencyChanged -= AnimateScoreText;
            _countTween?.Kill();
        }
        #endregion

        private void AnimateScoreText(PlayerCurrencySO currency, int newValue)
        {
            _countTween?.Kill();

            _countTween = DOVirtual.Int(
                _currentValue,
                newValue,
                _animationDuration,
                UpdateScoreText).OnComplete(PlayPunchAnimation);
        }

        private void UpdateScoreText(int value)
        {
            _currentValue = value;
            _scoreText.SetText("{0}", _currentValue);
        }

        private void PlayPunchAnimation()
        {
            _scoreText.transform
                .DOScale(_scaleAmount, _scaleDuration)
                .SetEase(Ease.OutQuad)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}
