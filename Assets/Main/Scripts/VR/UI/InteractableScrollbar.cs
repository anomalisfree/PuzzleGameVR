using System;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.VR.UI
{
    public class InteractableScrollbar : InteractableUI
    {
        private const float EndDeltaError = 0.05f;

        private float _xRaycastPointPos;
        private Scrollbar _scrollbar;

        protected override void UIStart()
        {
            _scrollbar = GetComponent<Scrollbar>();

            _xRaycastPointPos = GetComponent<RectTransform>().sizeDelta.x * (_scrollbar.value / 2 - _scrollbar.value);
            raycastPoint.anchoredPosition = new Vector2(_xRaycastPointPos, 0);
        }

        protected override void UIUpdate()
        {
            if (Math.Abs(raycastPoint.anchoredPosition.x - _xRaycastPointPos) > 0)
            {
                _xRaycastPointPos = raycastPoint.anchoredPosition.x;
                UpdateScrollbarValue();
            }
        }

        private void UpdateScrollbarValue()
        {
            _scrollbar.value = _xRaycastPointPos / GetComponent<RectTransform>().sizeDelta.x + 0.5f;
            if (_scrollbar.value < EndDeltaError) _scrollbar.value = 0;
            if (_scrollbar.value > 1 - EndDeltaError) _scrollbar.value = 1;
        }
    }
}