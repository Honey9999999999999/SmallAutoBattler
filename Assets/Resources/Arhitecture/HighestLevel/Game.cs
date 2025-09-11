using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arhitecture
{
    public class Game : MonoBehaviour
    {
        public static event Action OnInitialized;

        [SerializeField] private string firstSceneName;
        private static Game instance;
        private SceneBase scene;

        private List<SceneConfig> sceneConfigs;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            sceneConfigs = new List<SceneConfig>()
            {
                new FieldOfWarSceneConfig()
            };
        }
        private void Start()
        {
            StartCoroutine(LoadSceneAsync(firstSceneName));
        }

        public IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            scene = new SceneBase(GetConfig(firstSceneName));

            while (!loadOperation.isDone)
            {
                yield return null;
            }

            scene.OnCreate();
            yield return null;
            scene.OnInitialize();
            yield return null;
            scene.OnStart();
            yield return null;

            OnInitialized?.Invoke();
        }

        private SceneConfig GetConfig(string sceneName)
        {
            return sceneConfigs.Single(x => x.SceneName == sceneName);
        }

        public static T GetInteractor<T>() where T : Interactor
        {
            return instance.scene.GetInteractor<T>();
        }
    }
}