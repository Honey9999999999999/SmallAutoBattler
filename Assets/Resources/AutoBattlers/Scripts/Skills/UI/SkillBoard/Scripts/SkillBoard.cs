using UnityEngine;

namespace Autobattlers
{
    public class SkillBoard : MonoBehaviour
    {
        [SerializeField] private SkillButton buttonPrefab;
        private Player player;

        private void Start()
        {
            player = FieldOfWar.GetPlayer();

            foreach (var skill in player.Skills)
            {
                SkillButton button = Instantiate(buttonPrefab, transform);
                button.SetAction(() => skill.Invoke());
                skill.OnReloadTimeChanged += (float currentTime) => button.SetReloadState(currentTime, skill.ReloadTime);
                skill.OnReloaded += button.ClearReloadState;
                button.RequeredMP = skill.RequireMP;
            }

            player.OnDead.AddListener((AutoBattler _) =>
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            });
        }
    }
}