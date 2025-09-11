using AutoBattlers.AttackModifications;

namespace AutoBattlers
{
    public class Enemy : AutoBattler
    {
        public override bool TryFindTarget(out AutoBattler enemy)
        {
            enemy = FieldOfWar.GetPlayer();
            return enemy != null && enemy.IsAlive;
        }

        protected override void Attack()
        {
            if(animator != null)
            {
                animator.Play("Attack");
            }
            Target.GetDamage(Stats.AttackPower.GeneralValue);
        }

        protected override void Initialize()
        {
            base.Initialize();

            AttackModSystem.GetAttackMod<LifeStealMod>().Percent = 1.5f;
            AttackModSystem.GetAttackMod<LifeStealMod>().Chance = .25f;
        }
    }
}