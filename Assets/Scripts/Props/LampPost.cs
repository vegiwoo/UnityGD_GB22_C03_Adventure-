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
        
        private float switchLampDelay;

        private bool isNightfall;
        
        private Coroutine lampGlowCoroutine;
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
    }
}