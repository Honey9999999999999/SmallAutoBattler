using System;
using UnityEngine;

namespace Autobattlers
{
    public abstract class Skill
    {
        public event Action<float> OnReloadTimeChanged;
        public event Action OnReloaded;

        public float ReloadTime
        {
            get
            {
                return reloadTime;
            }
            set
            {
                reloadTime = Mathf.Max(0, value);
            }
        }
        private float reloadTime;

        public float RequireMP
        {
            get => requireMP;
            set => requireMP = Math.Max(0, value);
        }
        private float requireMP;

        protected Player owner;
        private readonly Timer reloadTimer;

        public Skill(Player owner)
        {
            this.owner = owner;
            reloadTimer = new Timer();
            reloadTimer.OnTick += (float currentTime) => OnReloadTimeChanged?.Invoke(currentTime);
            reloadTimer.OnStoped += () => OnReloaded?.Invoke();
        }

        public void Invoke()
        {
            if (!reloadTimer.IsRunning)
            {
                if (owner.TryGetMana(RequireMP))
                {
                    Do();
                    reloadTimer.Start(ReloadTime);
                }                
            }
        }

        protected abstract void Do();
    }
}