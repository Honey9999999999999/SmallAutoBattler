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
                [typeof(StunBolt)] = "AutoBattlers/Prefabs/Spellls/StunBolt",
                [typeof(PhisicalBolt)] = "AutoBattlers/Prefabs/Spellls/PhisicalBolt"
            };
            spellChanceMap = new(new List<KeyValuePair<float, Type>>()
            {
                new(.4f, typeof(PhisicalBolt)),
                new(.3f, typeof(FireBolt)),
                new(.3f, typeof(StunBolt))
            });
        }

        public void CastSpell(AutoBattler target)
        {
            MagicSpell spell = Game.Instantiate(Resources.Load<MagicSpell>(spellPathMap[spellChanceMap.GetValue()]), transform);
            spell.InitializeOwner(owner);
            Vector3 attackVector = (target.transform.position - castPosition.position).normalized;

            CoroutineManager.StartCoroutineAsynk(SpellFlightAsync(spell, attackVector));
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
    }
}
