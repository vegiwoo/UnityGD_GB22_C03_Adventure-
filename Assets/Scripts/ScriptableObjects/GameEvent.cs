using System.Collections.Generic;
using UnityEngine;
using GameDevLib;

// ReSharper disable once CheckNamespace
namespace C03_Adventure
{
    /// <summary>
    /// Represents a game event.
    /// </summary>
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject, ISubject
    {
        private readonly List<IObserver> _observers = new ();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.OnEventRaised(this);
            }
        }
    }
}