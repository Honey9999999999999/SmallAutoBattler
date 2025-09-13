using System;

namespace AutoBattlers.AttackModifications
{
    public abstract class AttackMod
    {
        public AttackMod(AutoBattler owner)
        {
            this.owner = owner;
        }

        protected AutoBattler owner;

        public DamageType DamageType { get; protected set; }
        public float Chance
        {
            get => chance;
            set => chance = Math.Max(0, value);
        }
        private float chance;

        public abstract void Do(Attack attack);
    }
}
