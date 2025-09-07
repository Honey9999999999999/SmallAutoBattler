using System;

namespace Autobattlers
{
    public abstract class StatusEffect
    {
        public event Action OnShutDown;

        protected AutoBattler owner;
        protected readonly Timer shutdownTimer;

        public StatusEffect(AutoBattler owner, float duration)
        {
            this.owner = owner;
            shutdownTimer = new();            
            owner.OnDead.AddListener((AutoBattler _) => shutdownTimer.Stop());
            shutdownTimer.OnStoped += () => OnShutDown?.Invoke();
            Initialize();
            shutdownTimer.Start(duration);            
        }

        protected abstract void Initialize();
    }
}