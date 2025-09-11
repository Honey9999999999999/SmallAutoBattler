using UnityEngine;

namespace AutoBattlers
{
    public class BangSkill : Skill
    {
        public float AttackSpeedFactor
        {
            get
            {
                return attackSpeedFactor;
            }
            set
            {
                attackSpeedFactor = Mathf.Max(0, value);
            }
        }
        private float attackSpeedFactor;

        public float AttackPowerFactor
        {
            get => attackPowerFactor;
            set
            {
                attackPowerFactor = Mathf.Max(0, value);
            }
        }
        private float attackPowerFactor;

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
                owner.Stats.AttackPerSec.AdditionalValue -= attackSpeed;
                owner.Stats.AttackPower.AdditionalValue -= attackPower;
            };
        }

        protected override void Do()
        {
            attackSpeed = Mathf.CeilToInt(owner.Stats.AttackPerSec.BaseValue * attackSpeedFactor);
            owner.Stats.AttackPerSec.AdditionalValue += attackSpeed;

            attackPower = Mathf.CeilToInt(owner.Stats.AttackPower.BaseValue * attackSpeedFactor);
            owner.Stats.AttackPower.AdditionalValue += attackPower;

            timer.Start(Duration);
        }
    }
}