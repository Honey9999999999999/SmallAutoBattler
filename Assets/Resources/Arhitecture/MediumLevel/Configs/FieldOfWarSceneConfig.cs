namespace Arhitecture
{
    public class FieldOfWarSceneConfig : SceneConfig
    {
        public override string SceneName => "FOWScene";

        public override InteractorsBase GetInteractorBase()
        {
            InteractorsBase interactorBase = base.GetInteractorBase();
            interactorBase.AddInteractor<FieldOfWarInteractor>();

            return interactorBase;
        }

        public override RepositoriesBase GetRepositoriesBase()
        {
            RepositoriesBase repositoriesBase = base.GetRepositoriesBase();
            repositoriesBase.AddRepository<PlayerRepository>();

            return repositoriesBase;
        }
    }
}
