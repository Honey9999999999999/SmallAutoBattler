using UnityEngine;

namespace Arhitecture
{
    public class MainCanvasInteractor : Interactor
    {
        public Canvas Canvas { get; private set; }
        private const string CanvasPath = "AutoBattlers/Prefabs/UI/Canvas";
        public override void OnCreate()
        {
            base.OnCreate();

            Canvas = Game.Instantiate(Resources.Load<Canvas>(CanvasPath));
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            Canvas.worldCamera = Camera.main;
        }

        public override void OnStart()
        {
            base.OnStart();
        }
    }
}
