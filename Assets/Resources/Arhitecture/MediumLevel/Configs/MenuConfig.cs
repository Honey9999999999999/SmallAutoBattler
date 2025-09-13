namespace Arhitecture
{
    public class MenuConfig : SceneConfig
    {
        public override string SceneName => "MenuScene";

        public override InteractorsBase GetInteractorBase()
        {
            InteractorsBase interactorBase = base.GetInteractorBase();

            interactorBase.AddInteractor<MenuInteractor>();

            return interactorBase;
        }
    }
}
