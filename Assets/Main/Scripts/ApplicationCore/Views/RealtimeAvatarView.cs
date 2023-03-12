using System.Collections;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.Controllers;
using Main.Scripts.ApplicationCore.RealtimeModels;
using Normal.Realtime;
using Normal.Realtime.Examples;
using RootMotion.FinalIK;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class RealtimeAvatarView : MonoBehaviour
    {
        [SerializeField] private GameObject avatarModel;
        [SerializeField] private RealtimeView realtimeView;
        [SerializeField] private List<GameObject> localHideObjects;
        [SerializeField] private Transform headTarget;
        [SerializeField] private Transform leftHandRoot;
        [SerializeField] private Transform leftHandTarget;
        [SerializeField] private Transform rightHandRoot;
        [SerializeField] private Transform rightHandTarget;
        [SerializeField] private PlayerAvatarData playerAvatarData;
        [SerializeField] private Transform modelPivot;
        [SerializeField] private VoiceMouthMove voiceMouthMove;
        [SerializeField] private RealtimeAvatarVoice realtimeAvatarVoice;
        [SerializeField] private RuntimeAnimatorController animatorController;

        private RealtimeMultiplayerController _realtimeMultiplayerController;
        private string _playerName;
        private Transform _avatarTransform;
        private float _avatarDefaultHeight;
        private GameObject _currentAvatar;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (realtimeView.isOwnedLocallySelf)
            {
                _realtimeMultiplayerController = ClientBase.Instance.GetController<RealtimeMultiplayerController>();
                _playerName = _realtimeMultiplayerController.PlayerName;
                playerAvatarData.SetUsername(_playerName);
                playerAvatarData.SetHeight(PlayerPrefs.GetFloat("HeadDefaultHeight"));
                _realtimeMultiplayerController.SetAvatarHands(transform, (leftHandRoot, rightHandRoot));

                foreach (var localHideObject in localHideObjects)
                {
                    localHideObject.layer = LayerMask.NameToLayer("Avatar");
                }
            }

            StartCoroutine(WaitForPlayerData());
        }

        private IEnumerator WaitForPlayerData()
        {
            while (string.IsNullOrEmpty(playerAvatarData.GetUsername()))
            {
                yield return null;
            }

            if (!realtimeView.isOwnedLocallySelf)
            {
                _playerName = playerAvatarData.GetUsername();
            }

            InitAvatarModel();
        }

        private void OnDestroy()
        {
            Destroy(_currentAvatar);
        }

        private void InitAvatarModel()
        {
            _currentAvatar = Instantiate(avatarModel);

            _avatarTransform = _currentAvatar.transform;
            _avatarDefaultHeight =
                _avatarTransform.Find("Unity compliant skeleton/hips/spine/chest/chest1/neck/head/eye.L").position.y;
            _avatarTransform.localScale =
                Vector3.one * playerAvatarData.GetHeight() / _avatarDefaultHeight;

            // avatar.AddComponent<EyeAnimationHandler>();
            // var voiceHandler = avatar.AddComponent<VoiceHandler>();
            // voiceHandler.AudioProvider = AudioProviderType.AudioClip;
            // voiceMouthMove.voiceHandler = voiceHandler;

            _currentAvatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;

            var vrik = _currentAvatar.GetComponent<VRIK>();
            vrik.solver.spine.headTarget = headTarget;
            vrik.solver.leftArm.target = leftHandTarget;
            vrik.solver.rightArm.target = rightHandTarget;
            vrik.solver.locomotion.mode = IKSolverVR.Locomotion.Mode.Animated;

            if (realtimeView.isOwnedLocallySelf)
            {
                _currentAvatar.layer = LayerMask.NameToLayer("Avatar");
            }
        }

        private void Update()
        {
            if (_avatarTransform != null)
            {
                var avatarPosition = _avatarTransform.position;
                avatarPosition = new Vector3(avatarPosition.x, modelPivot.position.y, avatarPosition.z);
                _avatarTransform.position = avatarPosition;
            }
        }

        public string GetPlayerName()
        {
            return _playerName;
        }

        public void BlockUser()
        {
            gameObject.SetActive(false);
        }

        public void UnblockUser()
        {
            gameObject.SetActive(true);
        }

        public void Mute()
        {
            realtimeAvatarVoice.enabled = false;
        }

        public void Unmute()
        {
            realtimeAvatarVoice.enabled = true;
        }
    }
}