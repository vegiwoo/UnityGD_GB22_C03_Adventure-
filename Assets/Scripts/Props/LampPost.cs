using System.Collections;
using GameDevLib;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace C03_Adventure 
{
    public class LampPost : MonoBehaviour, GameDevLib.IObserver<bool>
    {
        public GameBoolEvent nightfallEvent;
        
        [SerializeField]
        public ParticleSystem fireOnLampPost;
        private ParticleSystem[] _particles;
        
        [SerializeField]
        private Light lampPointLight;

        private const float LampIntenseMinBound = 3.5f;
        private const float LampIntenseMaxBound = 5.5f;
        
        private readonly float _switchLampDelay = Random.Range(1.0f, 3.0f);

        private bool _isNightfall;
        
        private Coroutine lampGlowCoroutine;
        private void Start()
        {
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
        
        public void OnEventRaised(ISubject<bool> subject, bool isNightfall)
        {
            _isNightfall = isNightfall;
            if (!_isNightfall) return;
            lampGlowCoroutine = StartCoroutine(LampGlowCoroutine());
        }

        /// <summary>
        /// Coroutine for turning on and off light of lamp.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LampGlowCoroutine()
        {
            // Turn on
            yield return new WaitForSeconds(_switchLampDelay);
            
            foreach (var particle in _particles)
            {
                particle.Play();
            }
            lampPointLight.enabled = true;
            
            while (_isNightfall)
            {
                var intensity = Random.Range(LampIntenseMinBound, LampIntenseMaxBound);
                lampPointLight.intensity = Mathf.Lerp(lampPointLight.intensity, intensity, Time.deltaTime);
                yield return null;
            }

            // Turn off 
            yield return new WaitForSeconds(_switchLampDelay);
            
            lampPointLight.enabled = false;
            foreach (var particle in _particles)
            {
                particle.Stop();
            }
            lampGlowCoroutine = null;
        }
    }
}