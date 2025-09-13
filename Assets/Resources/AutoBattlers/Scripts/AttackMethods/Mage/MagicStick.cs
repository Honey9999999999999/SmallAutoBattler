using System;
using System.Collections;
using System.Collections.Generic;
using Arhitecture;
using Tools;
using UnityEngine;

namespace AutoBattlers
{
    public class MagicStick : MonoBehaviour
    {
        private Dictionary<Type, string> spellPathMap;
        private RandomList<Type> spellChanceMap;
        private Dictionary<Type, bool> realoadingSpells;

        [SerializeField] private AutoBattler owner;
        [SerializeField] private Transform castPosition;

        private void Awake()
        {
            spellPathMap = new()
            {
                [typeof(FireBolt)] = "AutoBattlers/Prefabs/Spellls/FireBolt",
                [typeof(StunBolt)] = "AutoBattlers/Prefabs/Spellls/StunBolt",
                [typeof(PhisicalBolt)] = "AutoBattlers/Prefabs/Spellls/PhisicalBolt"
            };
            spellChanceMap = new(new List<KeyValuePair<float, Type>>()
            {
                new(.4f, typeof(PhisicalBolt)),
                new(.3f, typeof(FireBolt)),
                new(.3f, typeof(StunBolt))
            });

            realoadingSpells = new()
            {
                [typeof(PhisicalBolt)] = false,
                [typeof(FireBolt)] = false,
                [typeof(StunBolt)] = false
            };
        }

        public void CastSpell(AutoBattler target)
        {
            Type magicType;

            do
            {
                magicType = spellChanceMap.GetValue();
            }
            while (realoadingSpells[magicType]);

            MagicSpell spell = Game.Instantiate(Resources.Load<MagicSpell>(spellPathMap[magicType]), transform);

            spell.InitializeOwner(owner);
            Vector3 attackVector = (target.transform.position - castPosition.position).normalized;

            CoroutineManager.StartCoroutineAsynk(SpellFlightAsync(spell, attackVector));
            CoroutineManager.StartCoroutineAsynk(ReloadSpell(spell));
        }

        private IEnumerator SpellFlightAsync(MagicSpell spell, Vector3 direction)
        {
            bool isGoal = false;
            spell.OnTrigger += (AutoBattler _) => isGoal = true;

            while (!isGoal)
            {
                spell.transform.position += spell.GetProjectileSpeed() * Time.deltaTime * direction;

                yield return null;
            }
        }

        private IEnumerator ReloadSpell(MagicSpell spell)
        {
            realoadingSpells[spell.GetType()] = true;
            float value = spell.ReloadTime;

            while (value > 0)
            {
                yield return null;
                value -= Time.deltaTime;
            }

            realoadingSpells[spell.GetType()] = false;
        }
    }
}
