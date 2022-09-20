using System.Collections.Generic;
using UnityEngine;
using GameDevLib.Interfaces;

// ReSharper disable once CheckNamespace
namespace C03_Adventure
{
    /// <summary>
    /// Represents a game event.
    /// </summary>
    [CreateAssetMenu(fileName = "GameBoolEvent", menuName = "ScriptableEvents/GameBoolEvent", order = 0)]
    public class GameBoolEvent : ScriptableObject, ISubject<bool>
    {
        private readonly List<IObserver<bool>> _observers = new ();

        public void Attach(IObserver<bool> observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver<bool> observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(bool args)
        {
            foreach (var observer in _observers)
            {
                observer.OnEventRaised(this, args);
            }
        }
    }
}