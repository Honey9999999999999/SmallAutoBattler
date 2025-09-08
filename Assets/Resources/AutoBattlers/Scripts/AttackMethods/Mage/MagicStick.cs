using Arhitecture;
using Autobattlers;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace AutoBattlers
{
    public class MagicStick : MonoBehaviour
    {
        private Dictionary<Type, string> spellPathMap;
        private RandomList<Type> spellChanceMap;
        [SerializeField] private AutoBattler owner;
        [SerializeField] private Transform castPosition;

        private void Awake()
        {
            spellPathMap = new()
            {
                [typeof(FireBolt)] = "AutoBattlers/Prefabs/Spellls/FireBolt",
                [typeof(StunBolt)] = "AutoBattlers/Prefabs/Spellls/StunBolt"
            };
            spellChanceMap = new(new Dictionary<float, Type>()
            {
                [.7f] = typeof(FireBolt),
                [.3f] = typeof(StunBolt)
            });
        }

        public void CastSpell(AutoBattler target)
        {
            MagicSpell spell = Game.Instantiate(Resources.Load<MagicSpell>(spellPathMap[spellChanceMap.GetValue()]), transform);
            Vector3 attackVector = (target.transform.position - castPosition.position).normalized;

            CoroutineManager.StartCoroutineAsynk(SpellFlightAsync(spell, attackVector));
        }

        private IEnumerator SpellFlightAsync(MagicSpell spell, Vector3 direction)
        {
            bool isGoal = false;
            spell.OnTrigger += (AutoBattler _) => isGoal = true;

            while (!isGoal)
            {
                yield return null;

                spell.transform.position += spell.GetProjectileSpeed() * Time.deltaTime * direction;
            }            
        }
    }
}
