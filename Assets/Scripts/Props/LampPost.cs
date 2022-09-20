using System.Collections;
using GameDevLib.Interfaces;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace C03_Adventure 
{
    public class LampPost : MonoBehaviour, IObserver<bool>
    {
        #region Links
        
        public GameBoolEvent nightfallEvent;
        [SerializeField] public ParticleSystem fireOnLampPost;
        [SerializeField] private Light lampPointLight;
        #endregion
        
        #region Fields
        
        private const float LampIntenseMinBound = 3.5f;
        private const float LampIntenseMaxBound = 5.5f;
        
        private float switchLampDelay;

        private ParticleSystem[] _particles;
        
        private bool isNightfall;
        
        private Coroutine lampGlowCoroutine;
        
        #endregion

        #region MonoBehaviour mehods
        
        private void Start()
        {
            switchLampDelay = Random.Range(1.0f, 3.0f);
            
            _particles = fireOnLampPost.GetComponentsInChildren<ParticleSystem>();
            lampPointLight.intensity = 0.5f;
            
            foreach (var particle in _particles)
            {
                particle.Stop();
            }
            lampPointLight.enabled = false;
        }
        
        private void OnEnable()
        {
            nightfallEvent.Attach(this);
        }
        
        private void OnDisable()
        {
            nightfallEvent.Detach(this);
        }
        
        #endregion
        
        #region Functionality
        
        public void OnEventRaised(ISubject<bool> subject, bool nightfall)
        {
            isNightfall = nightfall;
            if (!this.isNightfall) return;
            lampGlowCoroutine = StartCoroutine(LampGlowCoroutine());
        }

        /// <summary>
        /// Coroutine for turning on and off light of lamp.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LampGlowCoroutine()
        {
            // Turn on
            yield return new WaitForSeconds(switchLampDelay);
            
            foreach (var particle in _particles)
            {
                particle.Play();
            }
            lampPointLight.enabled = true;
            
            while (isNightfall)
            {
                var intensity = Random.Range(LampIntenseMinBound, LampIntenseMaxBound);
                lampPointLight.intensity = Mathf.Lerp(lampPointLight.intensity, intensity, Time.deltaTime);
                yield return null;
            }

            // Turn off 
            yield return new WaitForSeconds(switchLampDelay);
            
            lampPointLight.enabled = false;
            foreach (var particle in _particles)
            {
                particle.Stop();
            }
            lampGlowCoroutine = null;
        }
        
        #endregion
    }
}