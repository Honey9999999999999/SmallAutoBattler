using UnityEngine;

namespace Autobattlers
{
    public class BangSkill : Skill
    {
        public float DecreaseAttackSpeed
        {
            get
            {
                return decreaseAttackSpeed;
            }
            set
            {
                decreaseAttackSpeed = Mathf.Clamp01(value);
            }
        }
        private float decreaseAttackSpeed;
        private float oldAttackSpeed;

        public float IncreaseAttack
        {
            get => increaseAttack;
            set
            {
                increaseAttack = Mathf.Max(0, value);
            }
        }
        private float increaseAttack;
        private float oldAttack;

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
                owner.attackPerSecond = oldAttackSpeed;
                owner.attackPower = oldAttack;
            };
        }

        protected override void Do()
        {
            oldAttackSpeed = owner.attackPerSecond;
            owner.attackPerSecond *= DecreaseAttackSpeed;
            oldAttack = owner.attackPower;
            owner.attackPower *= IncreaseAttack;

            timer.Start(Duration);
        }
    }
}