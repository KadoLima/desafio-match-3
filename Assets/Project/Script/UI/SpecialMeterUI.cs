using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Gazeus.DesafioMatch3.UI
{
    public class SpecialMeterUI : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Color _activeSpecialBarColor;

        [Header("SHOW UI PARAMETERS")]
        [Tooltip("Duration of the movement animation.")]
        [SerializeField] private float _showMoveDuration = 0.5f;

        private Color _defaultColor;

        private void Awake()
        {
            _fillImage.fillAmount = 0;
            _defaultColor = _fillImage.color;

            _content.anchoredPosition = new Vector2(0, 800);
        }

        public void Show() => _content.DOAnchorPosY(0, _showMoveDuration).SetEase(Ease.OutExpo);

        public void IncreaseFillAmount(float amountToAdd)
        {
            _fillImage.fillAmount = amountToAdd;
        }

        public void StartSpecial(float duration)
        {
            _fillImage.DOColor(_activeSpecialBarColor, 0.1f);
            _fillImage.DOFillAmount(0, duration).SetEase(Ease.Linear);
        }

        public void EndSpecial()
        {
            _fillImage.DOColor(_defaultColor, 0.1f);
            _fillImage.fillAmount = 0f;
        }
    }
}
