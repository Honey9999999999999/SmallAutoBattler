using System;
using System.Collections.Generic;

namespace Arhitecture
{
    public class InteractorBase
    {
        private readonly Dictionary<Type, Interactor> interactors;

        public InteractorBase()
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

        public T GetInteractor<T>() where T : Interactor
        {
            return (T)interactors[typeof(T)];
        }
    }
}
