using System;
using System.Collections;
using System.Collections.Generic;
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
        public UnityEvent<float> OnHealthChanged;
        public UnityEvent<AutoBattler> OnDead;
        public UnityEvent<AutoBattler> OnDispose;

        public AttackModSystem AttackModSystem { get; private set; }
        public StatusSystem StatusSystem { get; private set; }

        public RecoveryResource health;

        public bool IsAlive => health.IsResource;
        public bool IsStuned { get; private set; }

        [Min(0)] public float attackPower;
        [Min(0)] public float attackPerSecond;
        private float TimeBetweenAttacks => 1 / attackPerSecond;
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
                        attackRoutine ??= StartCoroutine(Attack());
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
            health.FullRestore();
            health.OnChanged += (float ratio) => OnHealthChanged?.Invoke(ratio);
            health.OnEnd += () => StartCoroutine(Dead());
            StatusSystem = new StatusSystem(this);
            StatusSystem.OnStatusChanged += (StatusType status) => IsStuned = StatusSystem.IsStatus<StunEffect>();
            AttackModSystem = new AttackModSystem(this);
        }

        public virtual void Start()
        {
            OnHealthChanged?.Invoke(1);
            FindTarget();
        }

        public abstract void FindTarget();

        public void GetDamage(float damage, IEnumerable<AttackMod> attackMods)
        {
            health.GetResource(damage);

            foreach (var attackMod in attackMods)
            {
                attackMod.Do(this);
            }
        }
        public void GetStatuses(List<KeyValuePair<StatusType, float>> statuses)
        {
            foreach (var kvp in statuses)
            {
                StatusSystem.AddStatusEffect(kvp);
            }
        }


        private IEnumerator Attack()
        {
            while (true)
            {
                if (!IsStuned)
                {
                    if (target.health.IsResource)
                    {
                        AutoBattler currentTarget = target;
                        currentTarget.GetDamage(attackPower, AttackModSystem.GetAttackMods());

                        if(currentTarget.health.IsResource)
                            currentTarget.GetStatuses(StatusSystem.GetStatuses());
                    }
                }

                yield return new WaitForSeconds(TimeBetweenAttacks);
            }
        }
        private IEnumerator Dead()
        {
            StopCoroutine(attackRoutine);

            if (animator != null)
            {
                animator.SetBool("IsDead", true);
            }

            OnDead?.Invoke(this);

            yield return new WaitForSeconds(2);

            OnDispose?.Invoke(this);
            Destroy(gameObject);
        }
    }
}

