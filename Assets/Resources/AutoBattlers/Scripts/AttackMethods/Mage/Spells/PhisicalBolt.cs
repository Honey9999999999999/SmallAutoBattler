namespace AutoBattlers
{
    public class PhisicalBolt : MagicSpell
    {
        public override void Do(AutoBattler target)
        {
            Attack attack = new Attack(target)
            {
                DamageType = DamageType.Phisical,
                Damage = power
            };

            target.GetDamage(attack);
        }

        public override void InitializeOwner(AutoBattler owner)
        {
            power *= owner.Stats.AttackPower.GeneralValue;
        }
    }
}
