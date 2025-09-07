using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Autobattlers
{
    public class StatusSystem
    {
        public enum StatusType
        {
            Stun,
            Fire
        }

        public event Action<StatusType> OnStatusChanged;

        private readonly AutoBattler owner;

        private readonly List<StatusEffect> statusEffects;
        private readonly Dictionary<StatusType, List<KeyValuePair<float, float>>> statusChanceMap;
        private readonly Dictionary<StatusType, Func<float, StatusEffect>> statusCreateMap;

        public StatusSystem(AutoBattler owner)
        {
            this.owner = owner;

            statusEffects = new List<StatusEffect>();
            statusChanceMap = new Dictionary<StatusType, List<KeyValuePair<float, float>>>()
            {
                [StatusType.Stun] = new(),
                [StatusType.Fire] = new()
            };
            statusCreateMap = new Dictionary<StatusType, Func<float, StatusEffect>>()
            {
                [StatusType.Stun] = (float duration) => new StunEffect(owner, duration),
                [StatusType.Fire] = (float duration) => new FireStatus(owner, duration)
            };
        }


        public void AddStatusEffect(KeyValuePair<StatusType, float> statusInfo)
        {
            statusEffects.Add(statusCreateMap[statusInfo.Key].Invoke(statusInfo.Value));
            StatusEffect status = statusEffects[^1];
            status.OnShutDown += () => statusEffects.Remove(status);
            status.OnShutDown += () => OnStatusChanged?.Invoke(statusInfo.Key);
            OnStatusChanged?.Invoke(statusInfo.Key);
        }
        public void AddStatusChance(StatusType type, float chance, float duration)
        {
            statusChanceMap[type].Add(new(Mathf.Clamp01(chance), Mathf.Max(0, duration)));
        }

        public List<KeyValuePair<StatusType, float>> GetStatuses()
        {
            List<KeyValuePair<StatusType, float>> statuses = new();
            foreach (var type in statusChanceMap.Keys)
            {
                statuses.AddRange(statusChanceMap[type].
                    Where(x => x.Key > UnityEngine.Random.Range(0, 1f)).
                    Select(x => new KeyValuePair<StatusType, float>(type, x.Value)));
            }

            return statuses;
        }

        public bool IsStatus<T>() where T : StatusEffect
        {
            return statusEffects.Where(x => x is T).Count() > 0;
        }
        public int Count<T>() where T : StatusEffect
        {
            return statusEffects.Where(x => x is T).Count();
        }
    }
}