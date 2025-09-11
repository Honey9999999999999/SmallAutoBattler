using System;
using UnityEngine;

namespace AutoBattlers
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class MagicSpell : MonoBehaviour
    {
        public event Action<AutoBattler> OnTrigger;

        [SerializeField, Min(0)] protected float power;
        [SerializeField, Min(0)] protected float projectileSpeed;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger && collision.gameObject.TryGetComponent(out AutoBattler battler))
            {
                OnTrigger?.Invoke(battler);
                if (battler.IsAlive)
                {
                    Do(battler);
                }
                Destroy(gameObject);
            }
        }

        public virtual void InitializeOwner(AutoBattler owner)
        {
            power *= owner.Stats.Intellegence.GeneralValue;
        }

        public abstract void Do(AutoBattler target);

        public float GetProjectileSpeed() => projectileSpeed;
    }
}
