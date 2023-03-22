using Main.Scripts.ApplicationCore.Views;
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

        private bool _isOpen;
        private Vector3 _startMenuScale;

        private void Start()
        {
            _startMenuScale = menu.localScale;

            if (openWatchAction != null)
            {
                openWatchAction.action.Enable();
                openWatchAction.action.performed += (ctx) => { OpenWatch(); };
            }
        }

        private void OpenWatch()
        {
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

        private void Update()
        {
            if (Camera.main != null) menu.LookAt(Camera.main.transform.position);

            menu.localScale =
                Vector3.MoveTowards(menu.localScale, !_isOpen ? Vector3.zero : _startMenuScale,
                    Time.deltaTime * 0.001f);
        }
    }
}