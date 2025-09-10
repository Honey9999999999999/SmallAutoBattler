using System;
using System.Collections;
using System.Collections.Generic;
using Arhitecture;
using AutoBattlers;
using AutoBattlers.AttackModifications;
using BarSystem;
using UnityEngine;
using UnityEngine.Events;
using static Autobattlers.StatusSystem;

namespace Autobattlers
{
    public abstract class AutoBattler : MonoBehaviour
    {
        public event Action<AutoBattler> OnTargetChanged;

        public UnityEvent<float, float> OnHealthChanged;
        public UnityEvent<float, float> OnManaChanged;

        public UnityEvent<AutoBattler> OnDead;
        public UnityEvent<AutoBattler> OnDispose;

        public AttackModSystem AttackModSystem { get; private set; }
        public StatusSystem Statuses { get; private set; }

        public RecoveryResource Health { get; private set; }
        public RecoveryResource Mana { get; private set; }

        public bool IsAlive => Health.IsResource;
        public bool IsStuned { get; private set; }

        public EntityStats Stats => stats;
        [SerializeField] private EntityStats stats;

        public AutoBattler Target
        {
            get
            {
                return target;
            }
            set
            {
                if (target != value)
                {
                    target = value;

                    if (target != null)
                    {
                        target.OnDead.AddListener((AutoBattler _) => FindTarget());                        
                        OnTargetChanged?.Invoke(target);
                    }
                    else
                    {
                        StopCoroutine(attackRoutine);
                    }
                }
            }
        }
        private AutoBattler target;

        [SerializeField] private Animator animator;
        private Coroutine attackRoutine;

        public void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            Health = new RecoveryResource(stats.MaxHealth, stats.HealthRegeneration, 1, false);
            Health.OnChanged += (float current, float max) => OnHealthChanged?.Invoke(current, max);
            Health.OnEnd += () => StartCoroutine(Dead());
            Stats.OnStatsChanged += () => Health.MaxResource = Stats.MaxHealth;

            Mana = new RecoveryResource(Stats.MaxMana, Stats.ManaRegeneration, 0);
            Mana.OnChanged += (float current, float max) => OnManaChanged?.Invoke(current, max);
            Stats.OnStatsChanged += () => Mana.MaxResource = Stats.MaxMana;

            Statuses = new StatusSystem(this);
            Statuses.OnStatusChanged += (StatusType status) => IsStuned = Statuses.IsStatus<StunEffect>();
            AttackModSystem = new AttackModSystem(this);

            Game.GetInteractor<FieldOfWarInteractor>().FieldOfWar.TimerToBattle.OnStartBattle += () => attackRoutine ??= StartCoroutine(AttackAsync());
        }

        public virtual void Start()
        {
            Health.FullRestore();
            Mana.FullRestore();

            FindTarget();
        }

        public abstract void FindTarget();

        public void GetDamage(float damage)
        {
            if (Health.IsResource)
            {
                Health.GetResource(damage);
            }            
        }
        public void GetStatuses(List<KeyValuePair<StatusType, float>> statuses)
        {
            foreach (var kvp in statuses)
            {
                Statuses.AddStatusEffect(kvp);
            }
            
        }
        public void GetStatus(StatusType type, float durability)
        {
            if (Health.IsResource)
            {
                Statuses.AddStatusEffect(type, durability);
            }
        }

        public bool TryGetMana(float value)
        {
            if (Mana.Resource >= value)
            {
                Mana.GetResource(value);
                return true;
            }

            return false;
        }


        private IEnumerator AttackAsync()
        {
            while (true)
            {
                if (!IsStuned)
                {
                    Attack();
                }

                yield return new WaitForSeconds(Stats.TimeBetweenAttacks);
            }
        }

        protected abstract void Attack();

        private IEnumerator Dead()
        {
            StopCoroutine(attackRoutine);

            if (animator != null)
            {
                animator.SetBool("IsDead", true);
            }

            OnDead?.Invoke(this);
            OnHealthChanged.RemoveAllListeners();
            OnManaChanged.RemoveAllListeners();
            OnDead.RemoveAllListeners();

            yield return new WaitForSeconds(2);

            OnDispose?.Invoke(this);
            Destroy(gameObject);
        }
    }
}

