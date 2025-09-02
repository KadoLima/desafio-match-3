using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public abstract class GameEventWithParameterSO_Base<T> : ScriptableObject
    {
        private readonly List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

        public void Raise(T payload)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(payload);
            }
        }

        public void RegisterListener(IGameEventListener<T> listener)
        {
            if (listener != null && !listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void UnregisterListener(IGameEventListener<T> listener)
        {
            if (listener != null)
            {
                listeners.Remove(listener);
            }
        }
    }

    public interface IGameEventListener<T>
    {
        void OnEventRaised(T payload);
    }
}
