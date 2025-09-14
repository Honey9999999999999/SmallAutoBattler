using UI.CCMenu;
using UnityEngine;

namespace Arhitecture
{
    public class CCMenuInteractor : Interactor
    {
        private const string PrefabPath = "UI/CCMenu/Prefabs/CCMenu";
        private CCMenu menu;

        public override void OnCreate()
        {
            menu = Game.Instantiate(Resources.Load<CCMenu>(PrefabPath), Game.GetInteractor<MainCanvasInteractor>().Canvas.transform);
            base.OnCreate();
        }

        public override void OnInitialize()
        {
            Game.GetRepository<PlayerRepository>().DeleteData();
            menu.Initialize();
            base.OnInitialize();
        }

        public override void OnStart()
        {
            base.OnStart();
        }
    }
}
