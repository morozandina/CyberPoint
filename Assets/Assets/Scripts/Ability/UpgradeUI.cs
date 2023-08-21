using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Utility;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Ability
{
    public class UpgradeUI : MonoBehaviour
    {
        public static Action ClearCache;
        public Ability ability;
        private List<Class.Ability> _usedAbility = new List<Class.Ability>();
        
        [SerializeField] private SpriteRenderer[] abilityUI;
        [SerializeField] private TextMeshPro title;
        [SerializeField] private TextMeshPro description;
        
        private System.Random random = new System.Random();

        private void Awake()
        {
            ClearCache = () => { _usedAbility.Clear(); };
        }

        private void OnDestroy()
        {
            ClearCache = null;
        }

        public void InsertToU()
        {
            title.text = "< Ability_name />";
            description.text = $"// --- // ------ // --- // *** \n     description\n// --- // ------ // - // ***";

            var probability = Random.Range(0, 2);
            
            if (probability < 1)
                for (var i = 0; i < abilityUI.Length; i++)
                {
                    var skill = RandomSkills();
                    _usedAbility.Add(skill);
                    abilityUI[i].sprite = skill.image;
                    abilityUI[i].GetComponent<Button>().onClick.RemoveAllListeners();
                    var i1 = i;
                    abilityUI[i].GetComponent<Button>().onClick.AddListener(() => ConfigureButton(abilityUI[i1].transform, skill));
                }
            else
            {
                var randomSkills = RandomSkills();
                var randomPower = RandomPower();
                var randomAbility = RandomAbility();
                
                _usedAbility.Add(randomSkills);
                abilityUI[0].sprite = randomSkills.image;
                abilityUI[0].GetComponent<Button>().onClick.RemoveAllListeners();
                abilityUI[0].GetComponent<Button>().onClick.AddListener(() => ConfigureButton(abilityUI[0].transform, randomSkills));

                if (randomPower != null && probability > 1)
                {
                    _usedAbility.Add(randomPower);
                    abilityUI[1].sprite = randomPower.image;
                    abilityUI[1].GetComponent<Button>().onClick.RemoveAllListeners();
                    abilityUI[1].GetComponent<Button>().onClick.AddListener(() => ConfigureButton(abilityUI[1].transform, randomPower));
                }
                else
                {
                    var randomSkills1 = RandomSkills();
                    _usedAbility.Add(randomSkills1);
                    abilityUI[1].sprite = randomSkills1.image;
                    abilityUI[1].GetComponent<Button>().onClick.RemoveAllListeners();
                    abilityUI[1].GetComponent<Button>().onClick.AddListener(() => ConfigureButton(abilityUI[1].transform, randomSkills1));
                }
                
                var randomSkills2 = RandomSkills();
                _usedAbility.Add(randomSkills2);
                abilityUI[2].sprite = randomSkills2.image;
                abilityUI[2].GetComponent<Button>().onClick.RemoveAllListeners();
                abilityUI[2].GetComponent<Button>().onClick.AddListener(() => ConfigureButton(abilityUI[2].transform, randomSkills2));
                
                if (randomAbility != null)
                {
                    _usedAbility.Add(randomAbility);
                    abilityUI[3].sprite = randomAbility.image;
                    abilityUI[3].GetComponent<Button>().onClick.RemoveAllListeners();
                    abilityUI[3].GetComponent<Button>().onClick.AddListener(() => ConfigureButton(abilityUI[3].transform, randomAbility));
                }
                else
                {
                    var randomSkills3 = RandomSkills();
                    _usedAbility.Add(randomSkills3);
                    abilityUI[3].sprite = randomSkills3.image;
                    abilityUI[3].GetComponent<Button>().onClick.RemoveAllListeners();
                    abilityUI[3].GetComponent<Button>().onClick.AddListener(() => ConfigureButton(abilityUI[3].transform, randomSkills3));
                }
            }
        }

        private Class.Ability RandomPower()
        {
            var randomPower = Random.Range(0, 100);
            var powerList = ability.powers.Where(x => x.probability < randomPower).ToList();
            
            if (powerList.Count < 1) return null;
            
            var powerIndex = random.Next(powerList.Count);
            var power = powerList[powerIndex];
            return _usedAbility.Contains(power) ? null : power;
        }

        private Class.Ability RandomSkills()
        {
            var skillIndex = random.Next(ability.skills.Count);
            return ability.skills[skillIndex];
        }
        
        private Class.Ability RandomAbility()
        {
            var randomAbility = Random.Range(0, 100);
            var abilityList = ability.abilities.Where(x => x.probability < randomAbility).ToList();
            
            if (abilityList.Count < 1) return null;
            
            var abilityIndex = random.Next(abilityList.Count);
            var ab = abilityList[abilityIndex];
            return _usedAbility.Contains(ab) ? null : ab;
        }

        private void ConfigureButton(Transform uiAbility, Class.Ability ab)
        {
            for (var i = 0; i < abilityUI.Length; i++)
                abilityUI[i].transform.GetChild(0).gameObject.SetActive(false);
            
            uiAbility.transform.GetChild(0).gameObject.SetActive(true);
            title.text = "< " + ab.name + " >";
            description.text = ab.description;
            
            
        }

        public async void ApplyOk()
        {
            await Task.Delay(600);
            foreach (var ab in abilityUI)
                ab.transform.GetChild(0).gameObject.SetActive(false);
            
            title.text = "< Ability_name />";
            description.text = $"// --- // ------ // --- // *** \n     description\n// --- // ------ // - // ***";
        }
    }
}
