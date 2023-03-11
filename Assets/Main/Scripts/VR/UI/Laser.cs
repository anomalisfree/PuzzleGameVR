using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Main.Scripts.VR.UI
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private LayerMask layerMaskUI;
        [SerializeField] private float maxDistance;
        [SerializeField] private GameObject ball;
        [SerializeField] private InputActionReference actionClick;
        [SerializeField] private InputActionReference actionHold;

        private InteractableUI _currentUI;
        private bool _isHolded;

        private void Start()
        {
            lineRenderer.positionCount = 2;

            if (actionClick != null)
            {
                actionClick.action.Enable();
                actionClick.action.performed += (ctx) => { PressDown(); };
            }

            if (actionHold != null)
            {
                actionHold.action.Enable();
                actionHold.action.performed += (ctx) => { _isHolded = ctx.ReadValue<float>() > 0.9f;};
            }
        }

        private void FixedUpdate()
        {
            var fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position, fwd, out var hit, maxDistance, layerMaskUI))
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
                ball.transform.position = hit.point;
                ball.SetActive(true);

                var newCurrentUI = hit.collider.GetComponent<InteractableUI>();

                if (newCurrentUI != null)
                {
                    if (_currentUI != null && _currentUI != newCurrentUI)
                    {
                        HighlightOffCurrentUI();
                    }

                    _currentUI = newCurrentUI;
                    _currentUI.HighlightOn();
                }
                else
                {
                    if (_currentUI != null)
                    {
                        HighlightOffCurrentUI();
                    }
                }
                
                if(_isHolded && _currentUI != null) _currentUI.Hold(ball.transform.position);
            }
            else
            {
                lineRenderer.enabled = false;
                ball.SetActive(false);

                if (_currentUI != null)
                {
                    HighlightOffCurrentUI();
                }
            }
        }

        private void PressDown()
        {
            if (_currentUI != null)
                _currentUI.Click(ball.transform.position);
        }

        private void HighlightOffCurrentUI()
        {
            if (_currentUI == null) return;

            _currentUI.HighlightOff();
            _currentUI = null;
        }
    }
}