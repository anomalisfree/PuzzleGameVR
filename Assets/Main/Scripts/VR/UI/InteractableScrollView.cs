using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.VR.UI
{
    public class InteractableScrollView : InteractableUI
    {
        private const int HoldTimeDelta = 5;
        
        private int _holdTimer;
        private ScrollRect _scrollRect;
        private Vector2 _startRaycastPointPos;
        private Vector2 _startScrollContentPointPos;
        private InteractableUI[] _interactableUIs;

        protected override void UIStart()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _interactableUIs = GetComponentsInChildren<InteractableUI>();

            foreach (var interactableUI in _interactableUIs)
            {
                interactableUI.onHold.AddListener(delegate { UpdateRaycastPointPosition(interactableUI); });
            }
        }

        private void UpdateRaycastPointPosition(InteractableUI interactableUI)
        {
            if (interactableUI == this) return;
            raycastPoint.transform.position = interactableUI.raycastPoint.transform.position;
            _startRaycastPointPos = raycastPoint.anchoredPosition;
        }

        protected override void UIUpdate()
        {
            if (_holdTimer > 0)
            {
                _holdTimer--;

                var raycastPointAnchoredPosition = raycastPoint.anchoredPosition;
                var contentAnchoredPosition = _scrollRect.content.anchoredPosition;

                var posX = _scrollRect.horizontal
                    ? _startScrollContentPointPos.x + (raycastPointAnchoredPosition.x - _startRaycastPointPos.x)
                    : contentAnchoredPosition.x;

                var posY = _scrollRect.vertical
                    ? _startScrollContentPointPos.y + (raycastPointAnchoredPosition.y - _startRaycastPointPos.y)
                    : contentAnchoredPosition.y;

                contentAnchoredPosition = new Vector2(posX, posY);
                _scrollRect.content.anchoredPosition = contentAnchoredPosition;
            }
            else
            {
                _startRaycastPointPos = raycastPoint.anchoredPosition;
                _startScrollContentPointPos = _scrollRect.content.anchoredPosition;
            }
        }

        protected override void UIHold()
        {
            _holdTimer = HoldTimeDelta;
        }
    }
}