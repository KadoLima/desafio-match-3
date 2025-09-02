using DG.Tweening;
using Gazeus.DesafioMatch3.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Gazeus.DesafioMatch3.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private PlayerCurrencySO _currencyToTrack;
        [SerializeField] private GeneralGameRulesSO _generalGameRulesSO;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private RectTransform _doublePointsIndicator;

        [Header("SCORE TEXT ANIMATION PARAMETERS")]
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _scaleAmount = 1.2f;
        [SerializeField] private float _scaleDuration = 0.1f;

        [Header("DOUBLE POINTS INDICATOR TWEEN PARAMETERS")]
        [SerializeField] private float _indicatorRotation = 10f;
        [SerializeField] private float _indicatorShowHideDuration = 0.25f;
        [SerializeField] private TextMeshProUGUI _indicatorAmountText;

        private int _currentValue = 0;
        private Tween _countTween;
        private Tween _doublePointsTween;

        #region Unity

        private void Awake()
        {
            _doublePointsIndicator.gameObject.SetActive(false);
            _indicatorAmountText.SetText("x" + _generalGameRulesSO.SpecialMeterMultiplier);
        }

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
            if (_currencyToTrack != currency) return;

            _countTween?.Kill();

            _countTween = DOVirtual.Int(_currentValue, newValue, _animationDuration, UpdateScoreText).OnComplete(PlayTextPunchAnimation);
        }

        private void UpdateScoreText(int value)
        {
            _currentValue = value;
            _scoreText.SetText("{0}", _currentValue);
        }

        private void PlayTextPunchAnimation()
        {
            _scoreText.transform.DOScale(_scaleAmount, _scaleDuration).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);
        }

        public void ShowDoublePointsIndicator()
        {
            _doublePointsIndicator.DOKill();
            _doublePointsTween?.Kill();

            _doublePointsIndicator.localScale = Vector3.zero;
            _doublePointsIndicator.gameObject.SetActive(true);

            _doublePointsIndicator.DOScale(1f, _indicatorShowHideDuration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                _doublePointsTween = _doublePointsIndicator
                    .DORotate(new Vector3(0, 0, _indicatorRotation), 0.3f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });
        }

        public void HideDoublePointsIndicator()
        {
            _doublePointsIndicator.DOKill();
            _doublePointsTween?.Kill();

            _doublePointsIndicator.DOScale(0f, _indicatorShowHideDuration).SetEase(Ease.InBack).OnComplete(() =>
            {
                _doublePointsIndicator.gameObject.SetActive(false);
            });
        }
    }
}
