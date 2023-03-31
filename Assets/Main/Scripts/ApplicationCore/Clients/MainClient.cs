using System;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Controllers;
using Main.Scripts.ApplicationCore.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts.ApplicationCore.Clients
{
    public class MainClient : ClientBase
    {
        #region Params

        //Controllers
        [Header("Controllers")] [SerializeField]
        private VrPlayerController vrPlayerController;

        [SerializeField] private SceneLoaderController sceneLoaderController;
        [SerializeField] private AudioFXController audioFXController;
        [SerializeField] private LoginController loginController;
        [SerializeField] private RealtimeMultiplayerController realtimeMultiplayerController;
        [SerializeField] private BundleLoaderController bundleLoaderController;
        [SerializeField] private TimelineTimerController timelineTimerController;
        [SerializeField] private LevelController levelController;
        [SerializeField] private PuzzleController puzzleController;


        //Services

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////

        #region Initialize

        protected override void InitializeServices()
        {
        }

        protected override void InitializeControllers()
        {
            Controllers.Add(vrPlayerController);
            Controllers.Add(sceneLoaderController);
            Controllers.Add(audioFXController);
            Controllers.Add(loginController);
            Controllers.Add(realtimeMultiplayerController);
            Controllers.Add(bundleLoaderController);
            Controllers.Add(timelineTimerController);
            Controllers.Add(levelController);
            Controllers.Add(puzzleController);
        }

        protected override void StartScenario()
        {
            InitializeVrPlayerController();
        }

        public override void LoadNewScene(string scene, Action onLoad, bool useBundle = false)
        {
            vrPlayerController.ResetPosition();

            sceneLoaderController.SceneIsLoaded = null;
            SceneManager.LoadScene(loadingSceneName);
            sceneLoaderController.SceneIsLoaded += onLoad.Invoke;

            if (useBundle)
                sceneLoaderController.InitFromBundle(scene);
            else
                sceneLoaderController.Init(scene);
        }

        public override void LoadNewScene(string scene, bool useBundle = false)
        {
            vrPlayerController.ResetPosition();

            SceneManager.LoadScene(loadingSceneName);

            if (useBundle)
                sceneLoaderController.InitFromBundle(scene);
            else
                sceneLoaderController.Init(scene);
        }

        public override void LoadNewScene(string scene, string room, bool useBundle = false)
        {
            vrPlayerController.ResetPosition();

            sceneLoaderController.SceneIsLoaded = null;
            SceneManager.LoadScene(loadingSceneName);
            SceneIsLoaded(room);

            if (useBundle)
                sceneLoaderController.InitFromBundle(scene);
            else
                sceneLoaderController.Init(scene);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////
        private Transform _playerRoot;
        private (Transform leftHandRoot, Transform rightHandRoot) _handRoots;
        private List<Transform> _vrikPoints;
        private string _playerName;
        private string _room;
        private Gender _gender;
        private Action _onFirstSceneLoad;

        private void InitializeVrPlayerController()
        {
            vrPlayerController.Ready += VrPlayerControllerReady;
            vrPlayerController.Init();
        }

        private void VrPlayerControllerReady(Transform playerRoot,
            (Transform leftHandRoot, Transform rightHandRoot) handRoots, List<Transform> vrikPoints)
        {
            vrPlayerController.Ready -= VrPlayerControllerReady;
            _playerRoot = playerRoot;
            _handRoots = handRoots;
            _vrikPoints = vrikPoints;
            _onFirstSceneLoad += InitializeLoginController;
            LoadNewScene(scenes[0], _onFirstSceneLoad);
        }

        private void InitializeLoginController()
        {
            loginController.IsLogin += OnLogin;
            loginController.Init();
        }
        
        private void OnLogin(string playerName, string room, Gender gender)
        {
            loginController.IsLogin -= OnLogin;
            _playerName = playerName;
            _room = room;
            _gender = gender;
            LoadNewScene(scenes[1], room);
        }
        
        private void SceneIsLoaded(string room)
        {
            if (string.IsNullOrEmpty(room)) return;

            realtimeMultiplayerController.Ready += RealtimeMultiplayerControllerReady;
            realtimeMultiplayerController.Init(room, _playerRoot, _handRoots, _vrikPoints, _playerName, _gender);
        }
        
        private void RealtimeMultiplayerControllerReady()
        {
            realtimeMultiplayerController.Ready -= RealtimeMultiplayerControllerReady;
            InitializeTimelineTimeController();
            InitializeLevelController();
            //InitializePuzzleController();
        }
        
        private void InitializeTimelineTimeController()
        {
            timelineTimerController.Init();
        }
        
        // private void InitializePuzzleController()
        // {
        //     puzzleController.Init();
        // }

        private void InitializeLevelController()
        {
            levelController.Init();
        }
    }
}