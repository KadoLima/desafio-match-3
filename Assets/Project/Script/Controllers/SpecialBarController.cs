using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3
{
    public class SpecialBarController : MonoBehaviour
    {
        [SerializeField] private GeneralGameRulesSO _generalGameRulesSO;

        [SerializeField] private UnityEvent<float> OnSpecialStartedEvent;
        [SerializeField] private UnityEvent OnSpecialEndEvent;
        [SerializeField] private UnityEvent<float> OnSpecialBarFillIncreasedEvent;


        private float _currentSpecialBarAmount = 0f;
        private bool _isTriggeringSpecial;
        
        private const float MAX_SPECIAL_BAR_AMOUNT = 1f;

        public bool IsTriggeringSpecial => _isTriggeringSpecial;

        public void IncreaseSpecialBarFill(float tilesDestroyed)
        {
            if (_isTriggeringSpecial) return;

            var multiplier = _generalGameRulesSO.FillPercentPerTileDestroyed / 100;

            _currentSpecialBarAmount += multiplier * tilesDestroyed;

            OnSpecialBarFillIncreasedEvent.Invoke(_currentSpecialBarAmount);

            if (_currentSpecialBarAmount >= MAX_SPECIAL_BAR_AMOUNT)
            {
                _currentSpecialBarAmount = MAX_SPECIAL_BAR_AMOUNT;
                StartSpecial();
            }
        }

        public void StartSpecial()
        {
            _isTriggeringSpecial = true;
            OnSpecialStartedEvent?.Invoke(_generalGameRulesSO.SpecialDurationSeconds);

            Invoke(nameof(EndSpecial), _generalGameRulesSO.SpecialDurationSeconds);
        }

        private void EndSpecial()
        {
            OnSpecialEndEvent?.Invoke();
            _isTriggeringSpecial = false;
            _currentSpecialBarAmount = 0;
        }
    }
}
