using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.Views;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Main.Scripts.VR.Other
{
    public class Watch : MonoBehaviour
    {
        [SerializeField] private InputActionReference openWatchAction;
        [SerializeField] private Transform menu;
        [SerializeField] private Image image;

        [SerializeField] private InputActionReference exitAction;

        private bool _isOpen;
        private Vector3 _startMenuScale;

        [SerializeField] private Transform exitMenu;

        [SerializeField] private TMP_Text countText;

        private bool _isExitMenuOpen;
        private Vector3 _startExitMenuScale;

        private float _exitMenuTimer;
        private int _exitMenuPressedCount = 5;


        private void Start()
        {
            _startMenuScale = menu.localScale;
            _startExitMenuScale = exitMenu.localScale;

            if (openWatchAction != null)
            {
                openWatchAction.action.Enable();
                openWatchAction.action.performed += (ctx) => { OpenWatch(); };
            }

            if (exitAction != null)
            {
                exitAction.action.Enable();
                exitAction.action.performed += (ctx) => { Exit(); };
            }
        }

        private void Exit()
        {
            _isExitMenuOpen = true;
            _exitMenuPressedCount--;
            _exitMenuTimer = 3;
            _isOpen = false;

            countText.text = $"Press the button {_exitMenuPressedCount} more times to exit.";

            if (_exitMenuPressedCount <= 0)
                Application.Quit();
        }

        private void OpenWatch()
        {
            _isExitMenuOpen = false;
            _exitMenuPressedCount = 5;
            
            var levelView = FindObjectOfType<LevelView>();

            if (levelView != null)
            {
                _isOpen = !_isOpen;

                if (_isOpen)
                {
                    image.sprite = levelView.GetImagePreview();
                }
            }
            else
            {
                _isOpen = false;
            }
        }

        public void CloseWatch()
        {
            _isOpen = false;
        }

        private void Update()
        {
            if (Camera.main != null) menu.LookAt(Camera.main.transform.position);

            menu.localScale =
                Vector3.MoveTowards(menu.localScale, !_isOpen ? Vector3.zero : _startMenuScale,
                    Time.deltaTime * 0.001f);

            if (Camera.main != null) exitMenu.LookAt(Camera.main.transform.position);

            exitMenu.localScale =
                Vector3.MoveTowards(exitMenu.localScale, !_isExitMenuOpen ? Vector3.zero : _startExitMenuScale,
                    Time.deltaTime * 0.001f);

            if (_exitMenuTimer > 0)
            {
                _exitMenuTimer -= Time.deltaTime;
            }
            else
            {
                _isExitMenuOpen = false;
                _exitMenuPressedCount = 5;
            }
        }
    }
}