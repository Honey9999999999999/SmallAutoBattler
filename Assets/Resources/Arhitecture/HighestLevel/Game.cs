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
                new MenuConfig(),
                new CCConfig(),
                new FieldOfWarSceneConfig()
            };
        }
        private void Start()
        {
            StartCoroutine(instance.LoadSceneAsync(firstSceneName));
        }

        public static void LoadScene(string sceneName)
        {
            OnInitialized = null;
            instance.StartCoroutine(instance.ReleaseResources());
            instance.StartCoroutine(instance.LoadSceneAsync(sceneName));
        }
        private IEnumerator ReleaseResources()
        {
            scene.OnDispose();
            yield return null;
        }
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            scene = new SceneBase(GetConfig(sceneName));

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
        public static T GetRepository<T>() where T : Repository
        {
            return instance.scene.GetRepository<T>();
        }
    }
}