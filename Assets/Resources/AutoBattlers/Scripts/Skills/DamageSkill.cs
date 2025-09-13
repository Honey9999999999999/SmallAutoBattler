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
            Attack attack = new Attack(owner.Target)
            {
                DamageType = DamageType.Magical,
                Damage = owner.Stats.AttackPower.GeneralValue * percent
            };

            owner.Target.GetDamage(attack);
        }
    }
}