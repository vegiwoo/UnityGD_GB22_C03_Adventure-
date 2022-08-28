using UnityEngine;
using UnityEngine.Events;
using GameDevLib;

// ReSharper disable once CheckNamespace
namespace C03_Adventure
{
    /// <summary>
    /// Some event observer.
    /// </summary>
    public class SomeObserver : MonoBehaviour, IObserver
    {
        public GameEvent someEvent;
        public UnityEvent response;

        private void OnEnable()
        {
            someEvent.Attach(this);
        }
        
        private void OnDisable()
        {
            someEvent.Detach(this);
        }

        public void OnEventRaised(ISubject subject)
        {
            response.Invoke();
            Debug.Log("This in new day!");
        }
    }
}