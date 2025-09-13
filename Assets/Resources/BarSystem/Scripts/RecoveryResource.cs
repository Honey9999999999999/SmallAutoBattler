using System;
using System.Collections;
using UnityEngine;

namespace BarSystem
{
    public class RecoveryResource
    {
        public RecoveryResource(float maxResource, float recoveryPower, float recoveryCooldown, bool canRestoreWhenEnd = true)
        {
            this.maxResource = maxResource;
            Resource = maxResource;
            this.recoveryPower = recoveryPower;
            this.recoveryCooldown = recoveryCooldown;
            CanRestoreWhenEnd = canRestoreWhenEnd;
        }
        public RecoveryResource() : this(100, 1, 1) { }

        public event Action<float, float> OnChanged;
        public event Action<float> OnGetted;
        public event Action<float> OnAdded;
        public event Action OnEnd;
        public event Action OnRestored;

        public float MaxResource
        {
            get => maxResource;
            set
            {
                float ratio = ResourceRatio;
                maxResource = Mathf.Max(0, value);
                ResourceRatio = ratio;
            }
        }
        private float maxResource;
        public float Resource
        {
            get { return resource; }
            private set
            {
                resource = Math.Clamp(value, 0, maxResource);
                OnChanged?.Invoke(resource, maxResource);
            }
        }
        private float resource;

        public float ResourceRatio
        {
            get => Resource / maxResource;
            set
            {
                Resource = maxResource * Mathf.Clamp01(value);
            }
        }

        private float recoveryPower;
        private float recoveryCooldown;

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

        public bool TryGetResource(float value)
        {
            if (Resource >= value)
            {
                GetResource(value);
                return true;
            }

            return false;
        }
        public void GetResource(float value)
        {
            value = Math.Clamp(value, 0, resource);
            Resource -= value;
            OnGetted?.Invoke(value);

            if (CanRestore)
            {
                if (recoveryRoutine != null)
                {
                    CoroutineManager.StopCoroutineAsynk(recoveryRoutine);
                }

                recoveryRoutine = CoroutineManager.StartCoroutineAsynk(RestoreAsynk());
            }

            if (!IsResource)
            {
                if (recoveryRoutine != null)
                {
                    CoroutineManager.StopCoroutineAsynk(recoveryRoutine);
                }

                OnEnd?.Invoke();
            }
        }

        private IEnumerator RestoreAsynk()
        {
            yield return new WaitForSeconds(recoveryCooldown);

            while (Resource < maxResource)
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
