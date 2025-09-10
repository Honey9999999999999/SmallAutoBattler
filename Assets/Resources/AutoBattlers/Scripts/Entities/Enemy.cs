using AutoBattlers.AttackModifications;

namespace Autobattlers
{
    public class Enemy : AutoBattler
    {
        public override void FindTarget()
        {
            Target = FieldOfWar.GetPlayer() != null && FieldOfWar.GetPlayer().IsAlive ? FieldOfWar.GetPlayer() : null;
        }

        protected override void Attack()
        {
            Target.GetDamage(Stats.AttackPower);
        }

        protected override void Initialize()
        {
            base.Initialize();

            AttackModSystem.GetAttackMod<LifeStealMod>().Percent = 1.5f;
            AttackModSystem.GetAttackMod<LifeStealMod>().Chance = .25f;
        }
    }
}