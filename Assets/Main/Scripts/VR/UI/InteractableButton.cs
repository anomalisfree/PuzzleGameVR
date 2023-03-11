using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Main.Scripts.VR.UI
{
    public class InteractableButton : InteractableUI
    {
        public UnityEvent switchToOn;
        public UnityEvent switchToOff;
        
        [SerializeField] private bool useHighlightsEffects;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color highlightColor = Color.white;
        [SerializeField] private Color onColor = Color.white;

        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite highlightSprite;
        [SerializeField] private Sprite onSprite;

        private Image _image;
        private bool _isOn;

        protected override void UIStart()
        {
            _image = GetComponent<Image>();
        }
        
        protected override void UIHighlightOn()
        {
            if(!useHighlightsEffects) return;

            if (!_isOn)
            {
                _image.color = highlightColor;
                if (highlightSprite != null) _image.sprite = highlightSprite;
            }
        }
        
        protected override void UIHighlightOff()
        {
            if(!useHighlightsEffects) return;
            
            if (_isOn)
            {
                _image.color = onColor;
                if (onSprite != null) _image.sprite = onSprite;
            }
            else
            {
                _image.color = defaultColor;
                if (defaultSprite != null) _image.sprite = defaultSprite;
            }
        }

        protected override void UIClick()
        {
            _isOn = !_isOn;
            
            if (_isOn)
            {
                if (useHighlightsEffects)
                {
                    _image.color = onColor;
                    if (onSprite != null) _image.sprite = onSprite;
                }

                switchToOn?.Invoke();
            }
            else
            {
                if (useHighlightsEffects)
                {
                    _image.color = defaultColor;
                    if (defaultSprite != null) _image.sprite = defaultSprite;
                }

                switchToOff?.Invoke();
            }
        }
    }
}