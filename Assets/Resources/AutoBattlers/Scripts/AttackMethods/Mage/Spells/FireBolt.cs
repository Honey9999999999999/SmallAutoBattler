namespace AutoBattlers
{
    public class FireBolt : MagicSpell
    {
        public override void Do(AutoBattler target)
        {
            target.GetStatus(StatusSystem.StatusType.Fire, power);
        }
    }
}
