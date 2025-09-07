using System;
using System.Collections;
using UnityEngine;

namespace BarSystem
{
    [Serializable]
    public class RecoveryResource
    {
        public RecoveryResource(float maxResource, float recoveryPower, bool canRestoreWhenEnd = true)
        {
            this.maxResource = maxResource;
            Resource = maxResource;
            this.recoveryPower = recoveryPower;
            CanRestoreWhenEnd = canRestoreWhenEnd;
        }
        public RecoveryResource() : this(100, 1) { }

        public event Action<float> OnChanged;
        public event Action<float> OnGetted;
        public event Action<float> OnAdded;
        public event Action OnEnd;
        public event Action OnRestored;

        [SerializeField, Min(0)] private float maxResource;
        public float Resource
        {
            get { return resource; }
            private set
            {
                resource = Math.Clamp(value, 0, maxResource);
                OnChanged?.Invoke(Resource / maxResource);
            }
        }
        private float resource;

        [SerializeField, Min(0)] private float recoveryPower;
        [SerializeField, Min(0)] private float recoveryCooldown;

        private Coroutine recoveryRoutine;

        public bool IsResource => Resource > 0;
        public bool CanRestoreWhenEnd;
        private bool CanRestore => (IsResource || CanRestoreWhenEnd) && recoveryPower > 0;

        public void AddResource(float value)
        {
            value = Math.Clamp(value, 0, maxResource - resource);
            Resource += value;
            OnAdded?.Invoke(value);
        }
        public void GetResource(float value)
        {
            value = Math.Clamp(value, 0, resource);
            Resource -= value;
            OnGetted?.Invoke(value);

            if (CanRestore)
            {
                if(recoveryRoutine != null)
                {
                    CoroutineManager.StopCoroutineAsynk(recoveryRoutine);
                }

                recoveryRoutine = CoroutineManager.StartCoroutineAsynk(RestoreAsynk());
            }

            if (!IsResource)
            {
                OnEnd?.Invoke();
            }
        }

        private IEnumerator RestoreAsynk()
        {
            yield return new WaitForSeconds(recoveryCooldown);

            while(Resource < maxResource)
            {
                Resource += recoveryPower * Time.deltaTime;
                yield return null;                
            }

            OnRestored?.Invoke();
        }

        public void FullRestore()
        {
            Resource = maxResource;
        }
    }
}
