namespace AutoBattlers
{
    public class FireStatus : StatusEffect
    {
        private float tickTime = .5f;

        public FireStatus(AutoBattler owner, float duration) : base(owner, duration) { }

        protected override void Initialize()
        {
            shutdownTimer.MaxTickTime = tickTime;
            shutdownTimer.OnTick += () => owner.GetDamage(2);
        }
    }
}