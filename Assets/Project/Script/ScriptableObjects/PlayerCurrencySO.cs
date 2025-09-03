using System;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Currency", menuName = "Currency")]
    public class PlayerCurrencySO : ScriptableObject
    {
        [Tooltip("Initial amount of the currency when the game starts.")]
        [SerializeField] private int _startingAmount = 0;
        [Tooltip("Maximum amount that can be stored.")]
        [SerializeField] private int _maxAmount = 99999;

        private int _currentAmount = 0;

        public int MaxAmount => _maxAmount;
        public int CurrentAmount => _currentAmount;



        public event Action<PlayerCurrencySO, int> OnCurrencyChanged;

        public void ResetCurrency()
        {
            _currentAmount = _startingAmount;

            OnCurrencyChanged?.Invoke(this, _currentAmount);
        }

        public void AddAmount(int amountToAdd)
        {
            _currentAmount += amountToAdd;
            _currentAmount = Mathf.Clamp(_currentAmount, 0, _maxAmount);

            OnCurrencyChanged?.Invoke(this, _currentAmount);
        }
    }
}
