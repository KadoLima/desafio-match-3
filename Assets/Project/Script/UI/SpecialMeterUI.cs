using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Gazeus.DesafioMatch3.UI
{
    public class SpecialMeterUI : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Color _activeSpecialBarColor;

        private Color _defaultColor;

        private void Awake()
        {
            _fillImage.fillAmount = 0;
            _defaultColor = _fillImage.color;
        }

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
