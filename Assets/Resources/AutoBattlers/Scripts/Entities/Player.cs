using System.Collections.Generic;
using System.Linq;
using BarSystem;
using UnityEngine.Events;

namespace Autobattlers
{
    public class Player : AutoBattler
    {
        public UnityEvent<float> OnManaChanged;

        public RecoveryResource mana;
        public List<Skill> Skills { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            mana.OnChanged += (float ratio) => OnManaChanged?.Invoke(ratio);            

            Skills = new()
            {
                new DamageSkill(this)
                {
                    Percent = 5,
                    ReloadTime = 1.3f,
                    RequireMP = 30
                },
                new BangSkill(this)
                {
                    ReloadTime = 20f,
                    DecreaseAttackSpeed = .4f,
                    IncreaseAttack = 2f,
                    Duration = 10f,
                    RequireMP = 60
                }
            };

            StatusSystem.AddStatusChance(StatusSystem.StatusType.Stun, .3f, 2);
            StatusSystem.AddStatusChance(StatusSystem.StatusType.Fire, .3f, 5);
        }

        public override void Start()
        {
            base.Start();
            mana.FullRestore();
        }

        public override void FindTarget()
        {
            if (TryGetWeaknessEnemy(out AutoBattler enemy))
            {
                Target = enemy;
                return;
            };

            Target = GetRandomEnemy();
        }

        public AutoBattler GetRandomEnemy()
        {
            IEnumerable<AutoBattler> enemies = FieldOfWar.GetEnemies();
            return enemies.ElementAt(UnityEngine.Random.Range(0, enemies.Count()));
        }

        public bool TryGetWeaknessEnemy(out AutoBattler enemy)
        {
            enemy = null;
            IEnumerable<AutoBattler> enemies = FieldOfWar.GetEnemies();

            if (enemies.Sum(x => x.health.Resource) / enemies.Count() != enemies.First().health.Resource)
            {
                enemy = enemies.OrderBy(x => x.health.Resource).First();
                return true;
            }

            return false;
        }
    }
}
