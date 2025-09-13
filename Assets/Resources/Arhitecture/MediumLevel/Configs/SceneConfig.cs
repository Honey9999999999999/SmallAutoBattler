namespace Arhitecture
{
    public abstract class SceneConfig
    {
        public abstract string SceneName { get; }
        public virtual InteractorsBase GetInteractorBase()
        {
            InteractorsBase interactorBase = new();
            interactorBase.AddInteractor<CoroutineInteractor>();
            interactorBase.AddInteractor<MainCanvasInteractor>();
            return interactorBase;
        }

        public virtual RepositoriesBase GetRepositoriesBase()
        {
            RepositoriesBase repositoriesBase = new();
            return repositoriesBase;
        }
    }
}
