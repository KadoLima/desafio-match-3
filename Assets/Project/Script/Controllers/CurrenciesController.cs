using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class CurrenciesController : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private List<PlayerCurrencySO> _playerCurrencies = new();
        [SerializeField] private GeneralGameRulesSO _generalGameRules;

        private float _currentMultiplier = 1f;

        #region Unity
        private void Start()
        {
            ResetValues();
        }
        #endregion

        private void ResetValues()
        {
            for (int i = 0; i < _playerCurrencies.Count; i++)
            {
                _playerCurrencies[i].ResetCurrency();
            }
        }

        public void AddAmount(PlayerCurrencySO currency, int tilesDestroyed = 0, bool shouldUseSpecialMultiplier = true)
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
                    var calculatedMultiplier = shouldUseSpecialMultiplier == true ? _currentMultiplier : 1;
                    var calculatedTilesDestroyed = tilesDestroyed == 0 ? 1 : tilesDestroyed;

                    var totalReward = Mathf.CeilToInt(_generalGameRules.GoldPerTile * calculatedTilesDestroyed * calculatedMultiplier);
                    _playerCurrencies[i].AddAmount(totalReward);
                }
            }
        }

        public void SetDefaultMeterMultiplier() => _currentMultiplier = 1;
        public void SetSpecialMeterMultiplier() => _currentMultiplier = _generalGameRules.SpecialMeterMultiplier;
    }
}
