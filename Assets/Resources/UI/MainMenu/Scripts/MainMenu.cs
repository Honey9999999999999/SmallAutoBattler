using Arhitecture;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartNewGame()
        {
            Game.LoadScene("CreateCharacterScene");
        }
        public void LoadGame()
        {
            Game.LoadScene("FOWScene");
        }
        public void ExitGame()
        {

        }
    }
}
