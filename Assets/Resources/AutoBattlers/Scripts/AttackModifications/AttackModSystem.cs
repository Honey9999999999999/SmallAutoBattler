using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoBattlers.AttackModifications
{
    public class AttackModSystem
    {
        private readonly Dictionary<Type, AttackMod> attackChanceMap;

        public AttackModSystem(AutoBattler owner)
        {
            attackChanceMap = new Dictionary<Type, AttackMod>()
            {
                [typeof(LifeStealMod)] = new LifeStealMod(owner)
            };
        }

        public IEnumerable<AttackMod> GetAttackMods()
        {
            return attackChanceMap.Values.Where(x => x.Chance > UnityEngine.Random.Range(0, 1));
        }

        public void AddAttackMod<T>(float chance) where T : AttackMod
        {
            attackChanceMap[typeof(T)].Chance += chance;
        }
        public T GetAttackMod<T>() where T : AttackMod
        {
            return (T)attackChanceMap[typeof(T)];
        }
    }
}
