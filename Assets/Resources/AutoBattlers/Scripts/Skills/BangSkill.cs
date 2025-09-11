using UnityEngine;

namespace AutoBattlers
{
    public class BangSkill : Skill
    {
        public float AttackSpeedFactor { get; set; }

        public float AttackPowerFactor { get; set; }

        private int attackSpeed;
        private int attackPower;

        public float Duration
        {
            get => duration;
            set
            {
                duration = Mathf.Max(0, value);
            }
        }
        private float duration;

        private readonly Timer timer;

        public BangSkill(Player owner) : base(owner)
        {
            timer = new Timer();
            timer.OnStoped += () =>
            {
                owner.Stats.AttackPerMin.AdditionalValue -= attackSpeed;
                owner.Stats.AttackPower.AdditionalValue -= attackPower;
            };
        }

        protected override void Do()
        {
            attackSpeed = Mathf.CeilToInt(owner.Stats.AttackPerMin.BaseValue * AttackSpeedFactor);
            owner.Stats.AttackPerMin.AdditionalValue += attackSpeed;

            attackPower = Mathf.CeilToInt(owner.Stats.AttackPower.BaseValue * AttackPowerFactor);
            owner.Stats.AttackPower.AdditionalValue += attackPower;

            timer.Start(Duration);
        }
    }
}