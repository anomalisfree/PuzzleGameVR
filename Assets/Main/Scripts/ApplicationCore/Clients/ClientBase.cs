using System;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Controllers;
using Main.Scripts.ApplicationCore.Services;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Clients
{
    public class ClientBase : MonoBehaviour
    {
        public string loadingSceneName;
        public List<string> scenes;
        public static ClientBase Instance { get; private set; }
        public List<BaseService> services = new List<BaseService>();
        protected List<BaseController> Controllers { get;} = new List<BaseController>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            InitializeServices();
            InitializeControllers();
            StartScenario();
        }

        protected virtual void InitializeServices()
        {
        }

        protected virtual void InitializeControllers()
        {
        }

        protected virtual void StartScenario()
        {
        }

        public virtual void LoadNewScene(string scene, bool useBundle = false)
        {
        }
        
        public virtual void LoadNewScene(string scene, Action onLoad, bool useBundle = false)
        {
        }
        
        public virtual void LoadNewScene(string scene, string room, bool useBundle = false)
        {
        }

        public T GetService<T>() where T : class
        {
            foreach (var service in services)
            {
                if (service is T serviceResult)
                {
                    return serviceResult;
                }
            }

            return null;
        }

        public T GetController<T>() where T : class
        {
            foreach (var controller in Controllers)
            {
                if (controller is T controllerResult)
                {
                    return controllerResult;
                }
            }

            return null;
        }
    }
}