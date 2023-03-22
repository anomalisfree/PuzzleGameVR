using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Scripts.VR.Other
{
    public class Watch : MonoBehaviour
    {
        [SerializeField] private InputActionReference openWatchAction;
        [SerializeField] private Transform menu;

        private void Start()
        {
            if (openWatchAction != null)
            {
                openWatchAction.action.Enable();
                openWatchAction.action.performed += (ctx) => { OpenWatch(); };
            }
        }

        private void OpenWatch()
        {
            Debug.Log("OpenWatch()");
        }

        private void Update()
        {
            if (Camera.main != null) menu.LookAt(Camera.main.transform.position);
        }
    }
}
