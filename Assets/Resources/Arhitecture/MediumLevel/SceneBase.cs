namespace Arhitecture
{
    public class SceneBase
    {
        public SceneBase(SceneConfig config)
        {
            interactorBase = config.GetInteractorBase();
            repositoriesBase = config.GetRepositoriesBase();
        }

        private readonly InteractorsBase interactorBase;
        private readonly RepositoriesBase repositoriesBase;

        public void OnCreate()
        {
            repositoriesBase.OnCreate();
            interactorBase.OnCreate();
        }

        public void OnInitialize()
        {
            interactorBase.OnInitialize();
        }

        public void OnStart()
        {
            interactorBase.OnStart();
        }

        public void OnDispose()
        {
            interactorBase.OnDispose();
        }

        public T GetInteractor<T>() where T : Interactor
        {
            return interactorBase.GetInteractor<T>();
        }
        public T GetRepository<T>() where T : Repository
        {
            return repositoriesBase.GetRepository<T>();
        }
    }
}
