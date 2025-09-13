using System;
using UnityEngine;

namespace Arhitecture
{
    public abstract class Repository
    {
        public event Action OnInitialized;

        public virtual void OnCreate()
        {
            bool isLoaded = LoadData();
            Debug.Log($"{GetType().Name}: Data {(isLoaded ? "" : "not ")}loaded.");
        }

        public abstract void SaveData();
        public abstract bool LoadData();
        public abstract void DeleteData();
    }
}
