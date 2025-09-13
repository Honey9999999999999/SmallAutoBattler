using System;
using System.Collections.Generic;

namespace Arhitecture
{
    public class InteractorsBase
    {
        private readonly Dictionary<Type, Interactor> interactors;

        public InteractorsBase()
        {
            interactors = new Dictionary<Type, Interactor>();
        }

        public void AddInteractor<T>() where T : Interactor, new()
        {
            interactors[typeof(T)] = new T();
        }

        public void OnCreate()
        {
            foreach (var interactor in interactors.Values)
            {
                interactor.OnCreate();
            }
        }

        public void OnInitialize()
        {
            foreach (var interactor in interactors.Values)
            {
                interactor.OnInitialize();
            }
        }

        public void OnStart()
        {
            foreach (var interactor in interactors.Values)
            {
                interactor.OnStart();
            }
        }

        public void OnDispose()
        {
            foreach (var interactor in interactors.Values)
            {
                interactor.OnDispose();
            }
        }

        public T GetInteractor<T>() where T : Interactor
        {
            return (T)interactors[typeof(T)];
        }
    }
}
