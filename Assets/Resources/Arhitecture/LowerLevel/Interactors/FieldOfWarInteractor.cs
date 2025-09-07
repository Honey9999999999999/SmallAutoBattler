using Autobattlers;
using UnityEngine;

namespace Arhitecture
{
    public class FieldOfWarInteractor : Interactor
    {
        public FieldOfWar FieldOfWar { get; private set; }
        private const string FieldOfWarPath = "AutoBattlers/Prefabs/FieldOfWar";

        public override void OnCreate()
        {
            FieldOfWar = Game.Instantiate(Resources.Load<FieldOfWar>(FieldOfWarPath), Game.GetInteractor<MainCanvasInteractor>().Canvas.transform);

            base.OnCreate();
        }

        public override void OnInitialize()
        {
            FieldOfWar.SpawnPlayer();
            FieldOfWar.SpawnEnemies();

            base.OnInitialize();
        }

        public override void OnStart()
        {
            base.OnStart();
        }
    }
}
