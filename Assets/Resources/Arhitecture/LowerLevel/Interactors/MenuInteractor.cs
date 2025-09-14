using UI.MainMenu;
using UnityEngine;

namespace Arhitecture
{
    public class MenuInteractor : Interactor
    {
        private const string PrefabPath = "UI/MainMenu/Prefabs/Menu";

        public MainMenu Menu { get; private set; }

        public override void OnCreate()
        {
            Menu = Game.Instantiate(Resources.Load<MainMenu>(PrefabPath), Game.GetInteractor<MainCanvasInteractor>().Canvas.transform);

            base.OnCreate();
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnStart()
        {
            base.OnStart();
        }
    }
}
