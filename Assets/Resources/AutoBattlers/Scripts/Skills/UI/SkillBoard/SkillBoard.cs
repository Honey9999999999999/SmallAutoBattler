using UnityEngine;

namespace Autobattlers
{
    public class SkillBoard : MonoBehaviour
    {
        [SerializeField] private SkillButton buttonPrefab;
        private Player player;

        private void Awake()
        {
            player = FieldOfWar.GetPlayer();
            player.OnDead.AddListener((AutoBattler _) => gameObject.SetActive(false));

            CreateSkillButtons();
        }

        private void CreateSkillButtons()
        {
            foreach (var skill in player.Skills)
            {
                SkillButton button = Instantiate(buttonPrefab, transform);
                button.SetAction(() => skill.Invoke());
                skill.OnReloadTimeChanged += (float currentTime) => button.SetReloadState(currentTime, skill.ReloadTime);
                skill.OnReloaded += button.ClearReloadState;
                button.RequeredMP = skill.RequireMP;
            }
        }
    }
}