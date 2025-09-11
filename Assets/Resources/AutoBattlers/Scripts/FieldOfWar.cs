using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AutoBattlers
{
    public class FieldOfWar : MonoBehaviour
    {
        public static event Action OnBattleStart;
        public static event Action OnEntitySpawned;

        [SerializeField] private Transform playerPoint;
        [SerializeField] private List<Transform> enemyPoint = new();
        private readonly Dictionary<AutoBattler, Transform> enemiesMap = new();

        [SerializeField] private Player playerPrefab;
        private Player player;
        [SerializeField] private AutoBattler enemyPrefab;

        public Transform targetImage;

        private static FieldOfWar instance;

        public static bool IsBattleStarted { get; private set; }

        public void Awake()
        {
            if(instance == null)
            {
                instance = this;

                return;
            }

            Destroy(gameObject);
        }

        public void StartBattle()
        {
            OnBattleStart?.Invoke();
            IsBattleStarted = true;
        }

        public void SpawnPlayer()
        {
            if (player == null)
            {
                player = Instantiate(playerPrefab, playerPoint);
                player.OnTargetChanged += MoveTargetImage;

                OnEntitySpawned?.Invoke();
            }
        }

        public static Player GetPlayer() => instance.player;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerEventData = new(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (var result in results)
                {
                    if (result.gameObject.TryGetComponent(out Enemy enemy) && enemy.IsAlive)
                    {
                        player.Target = enemy;
                    }
                }
            }
        }

        private void MoveTargetImage(AutoBattler enemy)
        {
            targetImage.position = enemiesMap[enemy].position;
        }

        public void SpawnEnemies()
        {
            for (int i = 0; i < enemyPoint.Count; i++)
            {
                SpawnEnemy(enemyPoint[i]);
            }
        }

        private void SpawnEnemy(Transform point)
        {
            if (enemiesMap.Values.Count(x => x == point) > 0)
            {
                return;
            }

            AutoBattler battler = Instantiate(enemyPrefab, point);
            enemiesMap[battler] = point;
            battler.OnDispose.AddListener(ReSpawnEnemy);

            OnEntitySpawned?.Invoke();
        }

        private void ReSpawnEnemy(AutoBattler battler)
        {
            Transform point = enemiesMap[battler];
            enemiesMap.Remove(battler);
            SpawnEnemy(point);
        }

        public static IEnumerable<AutoBattler> GetEnemies() => instance.enemiesMap.Keys.Where(x => x.Health.IsResource);
    }
}
