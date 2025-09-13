using System;
using System.Collections.Generic;

namespace Arhitecture
{
    public class RepositoriesBase
    {
        private readonly Dictionary<Type, Repository> repositories;

        public RepositoriesBase()
        {
            repositories = new Dictionary<Type, Repository>();
        }

        public void AddRepository<T>() where T : Repository, new()
        {
            repositories[typeof(T)] = new T();
        }

        public void OnCreate()
        {
            foreach (var interactor in repositories.Values)
            {
                interactor.OnCreate();
            }
        }

        public T GetRepository<T>() where T : Repository
        {
            return (T)repositories[typeof(T)];
        }
    }
}
