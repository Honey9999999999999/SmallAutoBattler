using System;

namespace AutoBattlers
{
    public class DamageSkill : Skill
    {
        public DamageSkill(Player owner) : base(owner) { }

        public float Percent
        {
            get => percent;
            set => percent = Math.Clamp(value, 0, 10);
        }
        private float percent;

        protected override void Do()
        {
            owner.Target.Health.GetResource(owner.Stats.AttackPower.GeneralValue * percent);
        }
    }
}