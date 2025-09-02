using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3
{
    public class FloatGameEventListener : MonoBehaviour, IGameEventListener<float>
    {
        [SerializeField] private FloatGameEventSO _event;
        [SerializeField] private UnityEvent<float> _response;
        [Tooltip("Delay before calling Response")]
        [SerializeField] private float _delay = 0;

        private float _cachedPayload = 0;

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

        public void OnEventRaised(float payload)
        {
            _cachedPayload = payload;
            Invoke(nameof(InvokeResponse), _delay);
        }

        private void InvokeResponse()
        {
            _response.Invoke(_cachedPayload);
        }
    }
}
