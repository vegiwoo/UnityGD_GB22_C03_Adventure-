using UnityEngine;

// ReSharper disable once CheckNamespace
namespace C03_Adventure.Stats
{
    /// <summary>
    /// Represents characteristics of a character.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats", order = 1)]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField] 
        public float maxHp;
    }
}