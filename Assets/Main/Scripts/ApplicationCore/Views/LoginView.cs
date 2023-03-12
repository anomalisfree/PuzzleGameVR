using System;
using System.Collections;
using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.Controllers;
using Main.Scripts.VR.UI;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class LoginView : MonoBehaviour
    {
        [SerializeField] private GameObject nameZone;
        [SerializeField] private GameObject roomZone;
        [SerializeField] private InputFieldVR inputFieldName;
        [SerializeField] private InteractableButton loginButton;
        [SerializeField] private InputFieldVR inputFieldRoom;
        [SerializeField] private InteractableButton connectButton;
        
        public Action<string,string> OnConnect;

        private float _currentZoneScale;
        private bool _isScaling;

        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
            connectButton.onClick.AddListener(OnConnectButtonClicked);

            nameZone.SetActive(true);
            roomZone.SetActive(false);
            nameZone.transform.localScale = Vector3.zero;
            _currentZoneScale = 0;
            _isScaling = true;

            if (PlayerPrefs.HasKey("PlayerName"))
            {
                inputFieldName.SetText(PlayerPrefs.GetString("PlayerName"));
            }

            if (PlayerPrefs.HasKey("RoomName"))
            {
                inputFieldRoom.SetText(PlayerPrefs.GetString("RoomName"));
            }

            ClientBase.Instance.GetController<AudioFXController>().AddAudioFX(transform, AudioFXType.FinalClick);
        }

        private void Update()
        {
            _currentZoneScale = _isScaling ? 0.005f : 0;
            roomZone.transform.localScale = nameZone.transform.localScale = Vector3.MoveTowards(
                nameZone.transform.localScale, Vector3.one * _currentZoneScale, Time.deltaTime * 0.05f);
        }

        private void OnLoginButtonClicked()
        {
            if (inputFieldName.GetText().Length == 0) return;

            StartCoroutine(OnLoginButtonClickedCor());
        }

        IEnumerator OnLoginButtonClickedCor()
        {
            ClientBase.Instance.GetController<AudioFXController>().AddAudioFX(transform, AudioFXType.FinalClick);
            _isScaling = false;
            PlayerPrefs.SetString("PlayerName", inputFieldName.GetText());

            yield return new WaitForSeconds(0.5f);

            nameZone.SetActive(false);
            roomZone.SetActive(true);

            ClientBase.Instance.GetController<AudioFXController>().AddAudioFX(transform, AudioFXType.FinalClick);
            _isScaling = true;
        }

        private void OnConnectButtonClicked()
        {
            if (inputFieldRoom.GetText().Length == 0) return;

            StartCoroutine(OnConnectButtonClickedCor());
        }

        IEnumerator OnConnectButtonClickedCor()
        {
            ClientBase.Instance.GetController<AudioFXController>().AddAudioFX(transform, AudioFXType.FinalClick);
            _isScaling = false;
            PlayerPrefs.SetString("RoomName", inputFieldRoom.GetText());

            yield return new WaitForSeconds(2f);

            nameZone.SetActive(false);
            roomZone.SetActive(false);
            
            OnConnect?.Invoke(inputFieldName.GetText(), inputFieldRoom.GetText());
        }
    }
}