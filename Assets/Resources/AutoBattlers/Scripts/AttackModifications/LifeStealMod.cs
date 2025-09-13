using System;

namespace AutoBattlers.AttackModifications
{
    public class LifeStealMod : AttackMod
    {
        public float Percent
        {
            get => percent;
            set => percent = Math.Clamp(value, 0, 10);
        }
        private float percent;

        public LifeStealMod(AutoBattler owner) : base(owner) { DamageType = DamageType.Phisical; }

        public override void Do(Attack attack)
        {
            attack.OnEnemyTakenDamage += (AutoBattler target, float value) => LifeSteal(value);
        }

        private void LifeSteal(float value)
        {
            owner.Health.AddResource(value * Percent);
        }
    }
}
