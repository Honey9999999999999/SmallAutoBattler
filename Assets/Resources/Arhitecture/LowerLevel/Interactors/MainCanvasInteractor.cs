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
        }

        public override void OnStart()
        {
            base.OnStart();
        }
    }
}
