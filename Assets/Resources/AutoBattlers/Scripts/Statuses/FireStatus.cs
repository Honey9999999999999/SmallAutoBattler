using UnityEngine;

namespace Autobattlers
{
    public class FireStatus : StatusEffect
    {
        private float tickTime = .5f;

        public FireStatus(AutoBattler owner, float duration) : base(owner, duration) { }

        protected override void Initialize()
        {
            shutdownTimer.TickTime = tickTime;
            shutdownTimer.OnTick += (float _) => owner.health.GetResource(2);
        }
    }
}