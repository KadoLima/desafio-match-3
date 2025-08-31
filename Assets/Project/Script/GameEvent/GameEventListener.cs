using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3.Effects
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEventSO _event;
        [SerializeField] private UnityEvent _response;
        [Tooltip("Delay before calling Response")]
        [SerializeField] private float _delay = 0;

        #region Unity
        public void OnEnable()
        {
            _event.RegisterListener(this);
        }

        private void OnDisable()
        {
            _event.UnregisterListener(this);
        }
        #endregion

        public void OnEventRaised()
        {
            StartCoroutine(OnEventRaised_Coroutine());
        }

        private IEnumerator OnEventRaised_Coroutine()
        {
            yield return new WaitForSeconds(_delay);
            _response.Invoke();
        }
    }
}
