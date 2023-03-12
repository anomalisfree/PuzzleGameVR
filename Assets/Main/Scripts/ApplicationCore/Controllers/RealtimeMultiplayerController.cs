using System;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Data;
using Main.Scripts.ApplicationCore.Views;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class RealtimeMultiplayerController : BaseController
    {
        [SerializeField] private RealtimeMultiplayerView realtimeMultiplayerView;

        private RealtimeMultiplayerView _realtimeMultiplayerView;
        private Transform _vrPlayerRoot;
        private Transform _vrAvatarRoot;

        private List<Transform> _handChildrenRight = new List<Transform>();
        private List<Transform> _handChildrenAvatarRight = new List<Transform>();
        private List<Transform> _handChildrenLeft = new List<Transform>();
        private List<Transform> _handChildrenAvatarLeft = new List<Transform>();

        private List<Transform> _vrikPoints;

        private string _playerName;
        private Gender _gender;

        public Action Ready;
        public string PlayerName => _playerName;
        public Gender Gender => _gender;

        public void Init(string roomName, Transform vrPlayerRoot,
            (Transform leftHandRoot, Transform rightHandRoot) handRoots, List<Transform> vrikPoints, string playerName, Gender gender)
        {
            //_networkSettings = networkSettings;
            _vrPlayerRoot = vrPlayerRoot;
            _vrikPoints = vrikPoints;
            _playerName = playerName;
            _gender = gender;

            if (_realtimeMultiplayerView == null)
            {
                _realtimeMultiplayerView = Instantiate(realtimeMultiplayerView);
                DontDestroyOnLoad(_realtimeMultiplayerView);

                if (handRoots.leftHandRoot != null)
                    _handChildrenLeft = GetAllChildren(handRoots.leftHandRoot, _handChildrenLeft);

                if (handRoots.rightHandRoot != null)
                    _handChildrenRight = GetAllChildren(handRoots.rightHandRoot, _handChildrenRight);
            }

            _realtimeMultiplayerView.ConnectToRoom(roomName);
        }

        public void Reset()
        {
            _vrPlayerRoot = null;
            _vrAvatarRoot = null;

            _handChildrenRight = new List<Transform>();
            _handChildrenAvatarRight = new List<Transform>();
            _handChildrenLeft = new List<Transform>();
            _handChildrenAvatarLeft = new List<Transform>();
        }

        public void Disconnect()
        {
            if (_realtimeMultiplayerView != null)
            {
                _realtimeMultiplayerView.DisconnectFromRoom();
            }
        }

        public void SetAvatarHands(Transform vrAvatarRoot,
            (Transform leftHandRoot, Transform rightHandRoot) handRootsAvatar)
        {
            _vrAvatarRoot = vrAvatarRoot;
            var (leftHandRoot, rightHandRoot) = handRootsAvatar;
            _handChildrenAvatarLeft = GetAllChildren(leftHandRoot, _handChildrenAvatarLeft);
            _handChildrenAvatarRight = GetAllChildren(rightHandRoot, _handChildrenAvatarRight);

            Ready?.Invoke();
        }

        public List<Transform> GetVrikPoints()
        {
            return _vrikPoints;
        }
        
        public Transform GetVrPlayerRoot()
        {
            return _vrPlayerRoot;
        }

        public RealtimeMultiplayerView GetRealtimeMultiplayerView()
        {
            return _realtimeMultiplayerView;
        }

        private static List<Transform> GetAllChildren(Transform parent, List<Transform> children)
        {
            if (parent.childCount <= 0) return children;

            foreach (Transform child in parent)
            {
                var realtimeViewComponent = child.GetComponent<RealtimeView>();

                if (realtimeViewComponent != null)
                    realtimeViewComponent.RequestOwnership();

                var realtimeTransformComponent = child.GetComponent<RealtimeTransform>();

                if (realtimeTransformComponent != null)
                    realtimeTransformComponent.RequestOwnership();

                children.Add(child);
                children = GetAllChildren(child, children);
            }

            return children;
        }

        private void Update()
        {
            if (_vrPlayerRoot != null && _vrAvatarRoot != null)
            {
                _vrAvatarRoot.position = _vrPlayerRoot.position;
                _vrAvatarRoot.rotation = _vrPlayerRoot.rotation;
            }

            _handChildrenLeft.RemoveAll(item => item == null);
            _handChildrenAvatarLeft.RemoveAll(item => item == null);
            _handChildrenRight.RemoveAll(item => item == null);
            _handChildrenAvatarRight.RemoveAll(item => item == null);


            foreach (var handChild in _handChildrenLeft)
            {
                foreach (var handChildAvatar in _handChildrenAvatarLeft)
                {
                    if (handChild.name == handChildAvatar.name)
                    {
                        handChildAvatar.position = handChild.position;
                        handChildAvatar.rotation = handChild.rotation;
                    }
                }
            }

            foreach (var handChild in _handChildrenRight)
            {
                foreach (var handChildAvatar in _handChildrenAvatarRight)
                {
                    if (handChild.name == handChildAvatar.name)
                    {
                        handChildAvatar.position = handChild.position;
                        handChildAvatar.rotation = handChild.rotation;
                    }
                }
            }
        }
    }
}