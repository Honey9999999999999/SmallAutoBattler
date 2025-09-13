using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoBattlers.AttackModifications
{
    public class AttackModSystem
    {
        private readonly Dictionary<Type, AttackMod> attackModsMap;

        public AttackModSystem(AutoBattler owner)
        {
            attackModsMap = new Dictionary<Type, AttackMod>()
            {
                [typeof(LifeStealMod)] = new LifeStealMod(owner)
            };
        }

        public IEnumerable<AttackMod> GetAttackMods()
        {
            return attackModsMap.Values.Where(x => x.Chance > UnityEngine.Random.Range(0, 1f));
        }

        public void AddAttackMod<T>(float chance) where T : AttackMod
        {
            attackModsMap[typeof(T)].Chance += chance;
        }
        public T GetAttackMod<T>() where T : AttackMod
        {
            return (T)attackModsMap[typeof(T)];
        }

        public void ApplyMods(Attack attack)
        {
            foreach (var mod in GetAttackMods().Where(x => x.DamageType == attack.DamageType))
            {
                mod.Do(attack);
            }
        }
    }
}
