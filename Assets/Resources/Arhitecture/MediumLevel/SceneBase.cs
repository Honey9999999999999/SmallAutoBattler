namespace Arhitecture
{
    public class SceneBase
    {
        public SceneBase(SceneConfig config)
        {
            interactorBase = config.GetInteractorBase();
        }

        private readonly InteractorBase interactorBase;

        public void OnCreate()
        {
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

        public T GetInteractor<T>() where T : Interactor
        {
            return interactorBase.GetInteractor<T>();
        }
    }
}
