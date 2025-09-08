using Autobattlers;

namespace AutoBattlers
{
    public class StunBolt : MagicSpell
    {
        public override void Do(AutoBattler battler)
        {
            if (battler.health.IsResource)
            {
                battler.GetStatus(StatusSystem.StatusType.Stun, power);
            }
        }
    }
}
