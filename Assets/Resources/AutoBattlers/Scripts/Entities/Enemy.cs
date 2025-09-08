using AutoBattlers.AttackModifications;

namespace Autobattlers
{
    internal class Enemy : AutoBattler
    {

        public override void FindTarget()
        {
            Target = FieldOfWar.GetPlayer() != null && FieldOfWar.GetPlayer().health.IsResource ? FieldOfWar.GetPlayer() : null;
        }

        protected override void Attack()
        {
            //if (Target.IsAlive)
            {
                Target.health.GetResource(Stats.AttackPower);
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            AttackModSystem.GetAttackMod<LifeStealMod>().Percent = 1.5f;
            AttackModSystem.GetAttackMod<LifeStealMod>().Chance = .25f;
        }
    }
}