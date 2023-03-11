using System;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Scripts.VR.UI
{
    public class InteractableUI : MonoBehaviour
    {
        public bool highlight3dEffect = true;
        public bool dynamicColliderSizeUpdate = true;
        public UnityEvent onClick;
        public UnityEvent onHold;
        
        [HideInInspector] 
        public RectTransform raycastPoint;

        private const float DeltaOnHighlight = 20f;

        private bool _canClick;
        private BoxCollider _boxCollider;
        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _boxCollider = GetComponent<BoxCollider>();
            raycastPoint = Instantiate(new GameObject("RaycastPoint"), transform).AddComponent<RectTransform>();
            
            UIStart();
        }

        protected virtual void UIStart()
        {
        }

        private void Update()
        {
            if (dynamicColliderSizeUpdate)
                UpdateBoxCollider();
            
            UIUpdate();
        }

        protected virtual void UIUpdate()
        {
        }

        private void UpdateBoxCollider()
        {
            if (_rectTransform != null)
            {
                if (!(Math.Abs(_rectTransform.sizeDelta.x - _boxCollider.size.x) > 0) &&
                    !(Math.Abs(_rectTransform.sizeDelta.y - _boxCollider.size.y) > 0))
                    return;

                var sizeDelta = _rectTransform.sizeDelta;
                _boxCollider.size = new Vector3(Math.Abs(sizeDelta.x), Math.Abs(sizeDelta.y), _boxCollider.size.z);

                var pivot = _rectTransform.pivot;
                _boxCollider.center = new Vector3((0.5f - pivot.x) * sizeDelta.x, (0.5f - pivot.y) * sizeDelta.y,
                    _boxCollider.center.z);
            }
        }

        public void HighlightOn()
        {
            if (highlight3dEffect)
                GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                    GetComponent<RectTransform>().anchoredPosition3D.x,
                    GetComponent<RectTransform>().anchoredPosition3D.y, -DeltaOnHighlight);

            _canClick = true;

            UIHighlightOn();
        }

        protected virtual void UIHighlightOn()
        {
        }

        public void HighlightOff()
        {
            if (highlight3dEffect)
                GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                    GetComponent<RectTransform>().anchoredPosition3D.x,
                    GetComponent<RectTransform>().anchoredPosition3D.y, 0f);

            _canClick = false;

            UIHighlightOff();
        }
        
        protected virtual void UIHighlightOff()
        {
        }

        public void Click(Vector3 hitPoint)
        {
            if (_canClick)
            {
                this.raycastPoint.transform.position = hitPoint;
                onClick.Invoke();
                UIClick();
            }
        }
        
        protected virtual void UIClick()
        {
        }

        public void Hold(Vector3 hitPoint)
        {
            if (_canClick)
            {
                this.raycastPoint.transform.position = hitPoint;
                onHold.Invoke();
                UIHold();
            }
        }
        
        protected virtual void UIHold()
        {
        }
    }
}