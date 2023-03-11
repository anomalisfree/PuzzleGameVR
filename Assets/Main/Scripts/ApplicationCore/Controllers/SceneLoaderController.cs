using System;
using System.Collections;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class SceneLoaderController : BaseController
    {
        [SerializeField] private SceneLoaderView sceneLoaderView;

        [Header("Objects")]
        [SerializeField] private GameObject exitTriggerPrefab;

        public Action SceneIsLoaded;

        private List<GameObject> _notDestroyedObjects;
        private SceneLoaderView _sceneLoaderView;

        public void Init(string scene, List<GameObject> notDestroyedObjects = null)
        {
            _notDestroyedObjects = notDestroyedObjects;

            if (_notDestroyedObjects != null)
                foreach (var notDestroyedObject in _notDestroyedObjects)
                {
                    DontDestroyOnLoad(notDestroyedObject);
                }

            StartLoadingScene(scene);
        }

        public void InitFromBundle(string scene, List<GameObject> notDestroyedObjects = null)
        {
            _notDestroyedObjects = notDestroyedObjects;

            if (_notDestroyedObjects != null)
                foreach (var notDestroyedObject in _notDestroyedObjects)
                {
                    DontDestroyOnLoad(notDestroyedObject);
                }

            _sceneLoaderView = Instantiate(sceneLoaderView);
            DontDestroyOnLoad(_sceneLoaderView);

            ClientBase.Instance.GetController<BundleLoaderController>()
                .LoadBundle($"4dsBundles/{scene}", () => StartCoroutine(LoadYourAsyncScene(scene)));
        }

        private void StartLoadingScene(string scene)
        {
            _sceneLoaderView = Instantiate(sceneLoaderView);
            DontDestroyOnLoad(_sceneLoaderView);
            StartCoroutine(LoadYourAsyncScene(scene));
        }

        private IEnumerator LoadYourAsyncScene(string scene)
        {
            _sceneLoaderView.SetSliderValue(0);
            yield return new WaitForSeconds(0.5f);

            var asyncLoad = SceneManager.LoadSceneAsync(scene);

            while (!asyncLoad.isDone)
            {
                if (_sceneLoaderView.GetSliderValue() < asyncLoad.progress)
                    _sceneLoaderView.SetSliderValue(asyncLoad.progress);

                yield return null;
            }

            if (_notDestroyedObjects != null)
                foreach (var notDestroyedObject in _notDestroyedObjects)
                {
                    Destroy(notDestroyedObject);
                }

            yield return UpdateSceneObjects();

            SceneIsLoaded?.Invoke();

            Destroy(_sceneLoaderView.gameObject);
        }

        private IEnumerator UpdateSceneObjects()
        {
            yield return UpdateExitTrigger();
        }

        private IEnumerator UpdateExitTrigger()
        {
            var exit = GameObject.Find("Exit");
            
            if (exit != null)
            {
               Instantiate(exitTriggerPrefab, exit.transform);
            }

            yield return null;
        }
    }
}