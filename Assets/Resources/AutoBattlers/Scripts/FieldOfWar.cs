using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Autobattlers
{
    public class FieldOfWar : MonoBehaviour
    {

        [SerializeField] private Transform playerPoint;
        [SerializeField] private List<Transform> enemyPoint = new();
        private readonly Dictionary<AutoBattler, Transform> enemiesMap = new();

        [SerializeField] private Player playerPrefab;
        private Player player;
        [SerializeField] private AutoBattler enemyPrefab;

        public Transform targetImage;

        private static FieldOfWar instance;

        public void Awake()
        {
            instance = instance != null ? instance : this;
        }

        public void SpawnPlayer()
        {
            if (player == null)
            {
                player = Instantiate(playerPrefab, playerPoint);
                player.OnTargetChanged += MoveTargetImage;
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
                    if (result.gameObject.TryGetComponent(out Enemy enemy))
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
            if (enemiesMap.Values.Where(x => x == point).Count() > 0)
            {
                return;
            }

            AutoBattler battler = Instantiate(enemyPrefab, point);
            enemiesMap[battler] = point;
            battler.OnDispose.AddListener(ReSpawnEnemy);
        }

        private void ReSpawnEnemy(AutoBattler battler)
        {
            Transform point = enemiesMap[battler];
            enemiesMap.Remove(battler);
            SpawnEnemy(point);
        }

        public static IEnumerable<AutoBattler> GetEnemies() => instance.enemiesMap.Keys.Where(x => x.IsAlive);
    }
}
