using System;
using System.Collections;
using UnityEngine;
using GameDevLib.Helpers;

// ReSharper disable once CheckNamespace
namespace C03_Adventure.Managers
{
    /// <summary>
    /// Day and night shift manager.
    /// </summary>
    public class DayNightShiftManager : MonoBehaviour
    {
        #region Links
        [SerializeField] private ParticleSystem starsParticles;
        [SerializeField] private GameBoolEvent nightfallEvent;
        #endregion
        
        #region Constants and variables
        private const float FullCircleInDegrees = 360f;
        
        private bool isNightfall;
        
        [field:SerializeField, ReadonlyField] 
        private float timeOfDay;
        
        [SerializeField] private float dayDuration; // in sec
        
        [Header("Sun")]
        [SerializeField] private Light sun;
        [SerializeField] private float sunIntensity;
        [SerializeField] private AnimationCurve sunCurve;
        
        [Header("Moon")]        
        [SerializeField] private Light moon;
        [SerializeField] private float moonIntensity;
        [SerializeField] private AnimationCurve moonCurve;
        
        [Header("SkyBoxes")]   
        [SerializeField] private Material daySkybox;
        [SerializeField] private Material nightSkybox;
        [SerializeField] private AnimationCurve skyBoxCurve;
        #endregion
        
        #region MonoBehaviour methods
        private void Start()
        {
            sunIntensity = sun.intensity;
            moonIntensity = moon.intensity;
            
            timeOfDay = 0f;
            isNightfall = false;

            StartCoroutine(TimeOfDayCoroutine());
        }
        #endregion
        
        #region Functionality
        
        private IEnumerator TimeOfDayCoroutine()
        {
            while (true)
            {
                timeOfDay = timeOfDay >= 1 ? 0 : timeOfDay += Time.deltaTime / dayDuration;
                
                sun.transform.localRotation = Quaternion.Euler(timeOfDay * FullCircleInDegrees, FullCircleInDegrees / 2,  0f);
                moon.transform.localRotation = Quaternion.Euler(timeOfDay * FullCircleInDegrees + FullCircleInDegrees / 2, FullCircleInDegrees / 2,  0f);
                
                sun.intensity = sunIntensity * sunCurve.Evaluate(timeOfDay);
                moon.intensity = moonIntensity * moonCurve.Evaluate(timeOfDay);

                RenderSettings.sun = skyBoxCurve.Evaluate(timeOfDay) > 0.1f ? sun : moon;
                RenderSettings.skybox.Lerp(nightSkybox, daySkybox, skyBoxCurve.Evaluate(timeOfDay));
                DynamicGI.UpdateEnvironment();

                var starsMain = starsParticles.main;
                starsMain.startColor = new Color(1, 1, 1, 1 - skyBoxCurve.Evaluate(timeOfDay));

                const float tolerance = 0.01f;
                switch (isNightfall)
                {
                    case false when Math.Abs(timeOfDay - 0.5f) < tolerance:
                        isNightfall = true;
                        nightfallEvent.Notify(true);
                        break;
                    case true when Math.Abs(timeOfDay - 1) < tolerance:
                        isNightfall = false;
                        nightfallEvent.Notify(false);
                        break;
                }
                
                yield return null;
            }
        }
        #endregion
    }
}