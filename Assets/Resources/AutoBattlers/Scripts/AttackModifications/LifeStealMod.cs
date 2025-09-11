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

        public LifeStealMod(AutoBattler owner) : base(owner) { }

        public override void Do(AutoBattler target)
        {
            owner.Health.AddResource(owner.Stats.AttackPower.GeneralValue * percent);
        }
    }
}
