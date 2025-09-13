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
                    AttackSpeedFactor = -.6f,
                    AttackPowerFactor = 2f,
                    Duration = 10f,
                    RequireMP = 60
                },
                new BangSkill(this)
                {
                    ReloadTime = 20f,
                    AttackSpeedFactor = 3f,
                    AttackPowerFactor = .4f,
                    Duration = 10f,
                    RequireMP = 120
                }
            };
        }

        public override bool TryFindTarget(out AutoBattler enemy)
        {
            if (TryGetWeaknessEnemy(out enemy) || TryGetRandomEnemy(out enemy))
            {
                Target = enemy;
                return true;
            };

            return false;
        }

        public bool TryGetRandomEnemy(out AutoBattler enemy)
        {
            enemy = null;
            IEnumerable<AutoBattler> enemies = FieldOfWar.GetEnemies();

            if (enemies.Count() > 0)
            {
                enemy = enemies.ElementAt(Random.Range(0, enemies.Count()));
                return true;
            }

            return false;
        }

        public bool TryGetWeaknessEnemy(out AutoBattler enemy)
        {
            enemy = null;
            IEnumerable<AutoBattler> enemies = FieldOfWar.GetEnemies();

            if (enemies.Count() > 0 && enemies.Sum(x => x.Health.Resource) / enemies.Count() != enemies.First().Health.Resource)
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
