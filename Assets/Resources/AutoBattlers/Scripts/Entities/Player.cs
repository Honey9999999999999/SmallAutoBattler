using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoBattlers
{
    public class Player : AutoBattler
    {
        [SerializeField] private MagicStick magicStick;
        public List<Skill> Skills { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

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
                    AttackSpeedFactor = .4f,
                    AttackPowerFactor = 2f,
                    Duration = 10f,
                    RequireMP = 60
                },
                new BangSkill(this)
                {
                    ReloadTime = 20f,
                    AttackSpeedFactor = 5f,
                    AttackPowerFactor = .4f,
                    Duration = 10f,
                    RequireMP = 120
                }
            };
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
            return enemies.ElementAt(Random.Range(0, enemies.Count()));
        }

        public bool TryGetWeaknessEnemy(out AutoBattler enemy)
        {
            enemy = null;
            IEnumerable<AutoBattler> enemies = FieldOfWar.GetEnemies();

            if (enemies.Sum(x => x.Health.Resource) / enemies.Count() != enemies.First().Health.Resource)
            {
                enemy = enemies.OrderBy(x => x.Health.Resource).First();
                return true;
            }

            return false;
        }

        protected override void Attack()
        {
            magicStick.CastSpell(Target);
        }
    }
}
