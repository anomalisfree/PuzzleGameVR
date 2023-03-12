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
        [SerializeField] private RealtimeView realtimeView;
        [SerializeField] private List<GameObject> localHideObjects;
        [SerializeField] private Transform headPivot;
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
        private bool _avatarLoaded;
        private string _playerName;
        private Transform _avatarTransform;
        private GameObject _defaultAvatar;
        private float _avatarDefaultHeight;
        private GameObject _currentAvatar;
        private bool _isVR;

        private const float DefaultHeight = 1.55f;
        private const float AvatarScaleDelta = 0.15f;

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

            if (_avatarLoaded) yield break;

            _avatarLoaded = true;
            LoadAvatar();
        }

        private void OnDestroy()
        {
            Destroy(_currentAvatar);
        }

        private void LoadAvatar()
        {
            //InitAvatarModel(avatar);
        }

        private void InitAvatarModel(GameObject avatar)
        {
            _avatarTransform = avatar.transform;
            _avatarDefaultHeight =
                _avatarTransform.Find("Armature/Hips/Spine/Spine1/Spine2/Neck/Head/LeftEye").position.y;
            _avatarTransform.localScale =
                Vector3.one * PlayerPrefs.GetFloat("HeadDefaultHeight") / _avatarDefaultHeight;
            _avatarTransform.Find("Armature/Hips/Spine/Spine1/Spine2/LeftShoulder/LeftArm/LeftForeArm/LeftHand")
                .localScale = Vector3.zero;
            _avatarTransform.Find("Armature/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand")
                .localScale = Vector3.zero;

            // avatar.AddComponent<EyeAnimationHandler>();
            // var voiceHandler = avatar.AddComponent<VoiceHandler>();
            // voiceHandler.AudioProvider = AudioProviderType.AudioClip;
            // voiceMouthMove.voiceHandler = voiceHandler;

            avatar.GetComponent<Animator>().runtimeAnimatorController = animatorController;

            var vrik = avatar.AddComponent<VRIK>();
            vrik.solver.spine.headTarget = headTarget;
            vrik.solver.leftArm.target = leftHandTarget;
            vrik.solver.rightArm.target = rightHandTarget;
            vrik.solver.locomotion.mode = IKSolverVR.Locomotion.Mode.Animated;

            if (realtimeView.isOwnedLocallySelf)
            {
                avatar.layer = LayerMask.NameToLayer("Avatar");
            }
        }

        private void Update()
        {
            if (_avatarTransform != null)
            {
                var avatarPosition = _avatarTransform.position;
                var avatarRotation = _avatarTransform.rotation;

                if (_isVR)
                {
                    avatarPosition = new Vector3(avatarPosition.x, modelPivot.position.y, avatarPosition.z);
                }
                else
                {
                    avatarPosition = transform.position + Vector3.down * DefaultHeight;
                    avatarRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }

                _avatarTransform.position = avatarPosition;
                _avatarTransform.rotation = avatarRotation;
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