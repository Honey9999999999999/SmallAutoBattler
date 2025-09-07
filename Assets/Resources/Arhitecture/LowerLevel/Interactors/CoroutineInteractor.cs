using UnityEngine;

namespace Arhitecture
{
    public class CoroutineInteractor : Interactor
    {
        private const string CoroutineManagerPath = "Tools/CoroutineManager/Prefabs/CoroutineManager";
        public override void OnCreate()
        {
            base.OnCreate();

            Game.Instantiate(Resources.Load<GameObject>(CoroutineManagerPath));
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
