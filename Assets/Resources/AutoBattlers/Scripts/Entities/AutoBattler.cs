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

        public EntityStats Stats => stats;
        [SerializeField] private EntityStats stats;
        public StatusSystem Statuses { get; private set; }
        public RecoveryResource Health { get; private set; }
        public RecoveryResource Mana { get; private set; }

        public bool IsAlive => Health.IsResource;
        public bool IsStuned
        {
            get => isStuned;
            set
            {
                if (isStuned != value)
                {
                    isStuned = value;

                    if (isStuned)
                    {
                        attackTimer.Pause();
                        return;
                    }
                    if (Target != null && Target.IsAlive)
                    {
                        attackTimer.Resume();
                    }
                }
            }
        }
        private bool isStuned;

        private Timer attackTimer;

        public AutoBattler Target
        {
            get
            {
                return target;
            }
            set
            {
                if (value != null && target != value)
                {
                    target = value;
                    OnTargetChanged?.Invoke(target);
                }
            }
        }
        private AutoBattler target;

        [SerializeField] protected Animator animator;

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
            Stats.AttackPerMin.OnChanged += (float _) => attackTimer.MaxTickTime = Stats.TimeBetweenAttacks;
        }

        public void Start()
        {
            Health.FullRestore();
            Mana.FullRestore();

            FindTarget();

            if (FieldOfWar.IsBattleStarted && Target != null)
            {
                attackTimer.StartTicks(Stats.TimeBetweenAttacks);
                return;
            }

            FieldOfWar.OnBattleStart += () => attackTimer.StartTicks(Stats.TimeBetweenAttacks);
        }

        private void FindTarget()
        {
            if(TryFindTarget(out AutoBattler enemy))
            {
                Target = enemy;
                Target.OnDead.AddListener((AutoBattler _) => FindTarget());
                FieldOfWar.OnEntitySpawned -= FindTarget;
                attackTimer.Resume();

                return;
            }

            if(Target != null)
            {
                attackTimer.Pause();
                FieldOfWar.OnEntitySpawned += FindTarget;
                Target = null;
            }
        }
        public abstract bool TryFindTarget(out AutoBattler enemy);

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
            Attack();

            foreach (var attackMod in AttackModSystem.GetAttackMods())
            {
                attackMod.Do(Target);
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

