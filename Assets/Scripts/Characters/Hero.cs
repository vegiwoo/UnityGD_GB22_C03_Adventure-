using C03_Adventure.Stats;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace C03_Adventure.Characters
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] 
        private CharacterStats heroStats;

        private float _currentHp;

        [SerializeField] private Texture2D hpIcon;
        public Color textureColor;
        
        private void Start()
        {
            _currentHp = heroStats.maxHp;
        }

        private void OnGUI()
        {
            // Hp bar
            var rect = new Rect(10, 10, 50, 30);  
            GUI.Box(rect, new GUIContent($" {_currentHp}", hpIcon));
            
            // Change color 
            textureColor = CreateRGBSlider(new Rect(10, 50, 200, 20), textureColor);
        }

        #region IMGUI
        
        private static Color CreateRGBSlider(Rect screenRect, Color rgb)
        {
            const float minValue = 0.0f;
            const float maxValue = 1.0f;
            
            rgb.r = CreateLabelSlider(screenRect, rgb.r, minValue, maxValue, "Red");
            screenRect.y += 20;
            rgb.g = CreateLabelSlider(screenRect, rgb.g, minValue, maxValue, "Green");
            screenRect.y += 20;
            rgb.b = CreateLabelSlider(screenRect, rgb.b, minValue, maxValue, "Blue");
            screenRect.y += 20;
            rgb.a = CreateLabelSlider(screenRect, rgb.a, minValue, maxValue, "Alpha");
            return rgb;
        }

        private static float CreateLabelSlider(Rect screenRect, float sliderValue, float minValue, float maxValue, string labelText)
        {
            GUI.Label(screenRect, labelText);
            screenRect.x += screenRect.width / 2;
         
            sliderValue = GUI.HorizontalSlider(screenRect, sliderValue, minValue, maxValue);
            return sliderValue;
        }
        
        #endregion
    }
}