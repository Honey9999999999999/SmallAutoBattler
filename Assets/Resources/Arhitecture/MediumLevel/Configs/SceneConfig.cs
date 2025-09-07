namespace Arhitecture
{
    public abstract class SceneConfig
    {
        public abstract string SceneName { get; }
        public virtual InteractorBase GetInteractorBase()
        {
            InteractorBase interactorBase = new();
            interactorBase.AddInteractor<CoroutineInteractor>();
            interactorBase.AddInteractor<MainCanvasInteractor>();
            return interactorBase;
        }
    }
}
