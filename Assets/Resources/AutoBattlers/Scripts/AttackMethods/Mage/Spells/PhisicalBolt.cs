namespace AutoBattlers
{
    public class PhisicalBolt : MagicSpell
    {
        public override void Do(AutoBattler target)
        {
            target.GetDamage(power);
        }

        public override void InitializeOwner(AutoBattler owner)
        {
            power *= owner.Stats.AttackPower.GeneralValue;
        }
    }
}
