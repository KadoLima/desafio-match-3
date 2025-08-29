using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class CurrenciesController : MonoBehaviour
    {
        [SerializeField] private List<PlayerCurrencySO> _playerCurrencies = new();

        private void Start()
        {
            ResetValues();
        }

        private void ResetValues()
        {
            for (int i = 0; i < _playerCurrencies.Count; i++)
            {
                _playerCurrencies[i].ResetCurrency();
            }
        }

        public void AddAmount(PlayerCurrencySO currency, int amountToAdd)
        {
            if (!_playerCurrencies.Contains(currency))
            {
                Debug.LogWarning($"Currency {currency.name} not registered in CurrenciesController");
                return;
            }

            for (int i = 0; i < _playerCurrencies.Count; i++)
            {
                if (_playerCurrencies[i] == currency)
                {
                    _playerCurrencies[i].AddAmount(amountToAdd);
                }
            }
        }
    }
}
