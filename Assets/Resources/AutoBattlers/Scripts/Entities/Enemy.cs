using AutoBattlers.AttackModifications;

namespace AutoBattlers
{
    public class Enemy : AutoBattler
    {
        public void Awake()
        {
            Initialize();
        }

        public override bool TryFindTarget(out AutoBattler enemy)
        {
            enemy = FieldOfWar.GetPlayer();
            return enemy != null && enemy.IsAlive;
        }

        protected override void Attack()
        {
            if (animator != null)
            {
                animator.Play("Attack");
            }

            Attack attack = new(Target)
            {
                DamageType = DamageType.Phisical,
                Damage = Stats.AttackPower.GeneralValue
            };

            AttackModSystem.ApplyMods(attack);
            Target.GetDamage(attack);
        }

        protected override void Initialize()
        {
            Stats.Initialize();

            base.Initialize();

            AttackModSystem.GetAttackMod<LifeStealMod>().Percent = 1.5f;
            AttackModSystem.GetAttackMod<LifeStealMod>().Chance = .25f;
        }
    }
}