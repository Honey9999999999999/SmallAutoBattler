namespace Arhitecture
{
    public class CCConfig : SceneConfig
    {
        public override string SceneName => "CreateCharacterScene";

        public override InteractorsBase GetInteractorBase()
        {
            InteractorsBase interactorsBase = base.GetInteractorBase();
            interactorsBase.AddInteractor<CCMenuInteractor>();

            return interactorsBase;
        }

        public override RepositoriesBase GetRepositoriesBase()
        {
            RepositoriesBase repositoriesBase = base.GetRepositoriesBase();
            repositoriesBase.AddRepository<PlayerRepository>();

            return repositoriesBase;
        }
    }
}
