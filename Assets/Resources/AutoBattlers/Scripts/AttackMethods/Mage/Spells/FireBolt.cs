using Autobattlers;

namespace AutoBattlers
{
    public class FireBolt : MagicSpell
    {
        public override void Do(AutoBattler battler)
        {
            if (battler.health.IsResource)
            {
                battler.GetStatus(StatusSystem.StatusType.Fire, power);
            }
        }
    }
}
