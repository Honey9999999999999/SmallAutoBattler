namespace Arhitecture
{
    public class FieldOfWarSceneConfig : SceneConfig
    {
        public override string SceneName => "FOWScene";

        public override InteractorBase GetInteractorBase()
        {
            InteractorBase interactorBase = base.GetInteractorBase();
            interactorBase.AddInteractor<FieldOfWarInteractor>();

            return interactorBase;
        }
    }
}
