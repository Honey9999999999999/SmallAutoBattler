using System;
using UnityEngine;

namespace Arhitecture
{
    public abstract class Interactor
    {
        public event Action OnInitialized;

        public virtual void OnCreate() { Debug.Log(GetType().Name + " is created"); }
        public virtual void OnInitialize() { Debug.Log(GetType().Name + " is initialized"); }
        public virtual void OnStart() { OnInitialized?.Invoke(); Debug.Log(GetType().Name + " is started"); }
    }
}
