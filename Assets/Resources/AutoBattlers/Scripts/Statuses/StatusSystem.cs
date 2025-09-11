using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoBattlers
{
    public class StatusSystem
    {
        public enum StatusType
        {
            Stun,
            Fire
        }

        public event Action<StatusType> OnStatusChanged;

        private readonly List<StatusEffect> statusEffects;
        private readonly Dictionary<StatusType, Func<float, StatusEffect>> statusCreateMap;

        public StatusSystem(AutoBattler owner)
        {
            statusEffects = new List<StatusEffect>();
            statusCreateMap = new Dictionary<StatusType, Func<float, StatusEffect>>()
            {
                [StatusType.Stun] = (float duration) => new StunEffect(owner, duration),
                [StatusType.Fire] = (float duration) => new FireStatus(owner, duration)
            };

            owner.OnDispose.AddListener((AutoBattler _) => OnStatusChanged = null);
        }

        public void AddStatusEffect(KeyValuePair<StatusType, float> statusInfo)
        {
            AddStatusEffect(statusInfo.Key, statusInfo.Value);
        }
        public void AddStatusEffect(StatusType type, float durability)
        {
            statusEffects.Add(statusCreateMap[type].Invoke(durability));
            StatusEffect status = statusEffects[^1];
            status.OnShutDown += () => statusEffects.Remove(status);
            status.OnShutDown += () => OnStatusChanged?.Invoke(type);
            OnStatusChanged?.Invoke(type);
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