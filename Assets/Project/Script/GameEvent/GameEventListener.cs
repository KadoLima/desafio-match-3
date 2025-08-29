using Gazeus.DesafioMatch3.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3.Effects
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEventSO Event;
        [SerializeField] private UnityEvent Response;
        [Tooltip("Delay before calling Response")]
        [SerializeField] private float delay = 0;

        public void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            StartCoroutine(OnEventRaised_Coroutine());
        }

        private IEnumerator OnEventRaised_Coroutine()
        {
            yield return new WaitForSeconds(delay);
            Response.Invoke();
        }
    }
}
