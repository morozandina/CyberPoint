using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Ability
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Objects/New Ability Data", order = 2)]
    public class Ability : ScriptableObject
    {
        [Header("Skills")] public List<Class.Ability> skills = new List<Class.Ability>();
        [Header("Abilities")] public List<Class.Ability> abilities = new List<Class.Ability>();
        [Header("Powers")] public List<Class.Ability> powers = new List<Class.Ability>();
    }
}
