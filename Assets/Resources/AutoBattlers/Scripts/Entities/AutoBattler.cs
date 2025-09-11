using Arhitecture;
using AutoBattlers.AttackModifications;
using BarSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static AutoBattlers.StatusSystem;

namespace AutoBattlers
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

        private Timer attackTimer;

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
                        attackTimer.Reset();
                    }
                }
            }
        }
        private AutoBattler target;

        [SerializeField] private Animator animator;

        public void Awake()
        {
           Initialize();
        }

        protected virtual void Initialize()
        {
            Stats.Initialize();

            Health = new RecoveryResource(stats.MaxHealth.GeneralValue, stats.HealthRegeneration.GeneralValue, 1, false);
            Health.OnChanged += (float current, float max) => OnHealthChanged?.Invoke(current, max);
            Health.OnEnd += () => StartCoroutine(Dead());
            Stats.MaxHealth.OnChanged += (float value) => Health.MaxResource = value;

            Mana = new RecoveryResource(Stats.MaxMana.GeneralValue, Stats.ManaRegeneration.GeneralValue, 0);
            Mana.OnChanged += (float current, float max) => OnManaChanged?.Invoke(current, max);
            Stats.MaxMana.OnChanged += (float value) => Mana.MaxResource = value;

            Statuses = new StatusSystem(this);
            Statuses.OnStatusChanged += (StatusType status) => IsStuned = Statuses.IsStatus<StunEffect>();
            AttackModSystem = new AttackModSystem(this);

            attackTimer = new Timer();
            attackTimer.OnTick += AttackTick;
            Stats.AttackPerSec.OnChanged += (float _) => attackTimer.MaxTickTime = Stats.TimeBetweenAttacks;

            Game.GetInteractor<FieldOfWarInteractor>().FieldOfWar.TimerToBattle.OnStartBattle += () => attackTimer.StartTicks(Stats.TimeBetweenAttacks);
        }

        public void Start()
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


        private void AttackTick()
        {
            if (!IsStuned)
            {
                Attack();
            }
        }

        protected abstract void Attack();

        private IEnumerator Dead()
        {
            attackTimer.Reset();

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

