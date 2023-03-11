using TMPro;
using UnityEngine;

namespace Main.Scripts.VR.UI
{
    public class InputFieldVR : MonoBehaviour
    {
        [SerializeField] private KeyboardVR keyboard;
        [SerializeField] private GameObject title;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private int maxSymbols = 30;
        [SerializeField] private bool hide;

        private string _text;
        
        public void OnClick()
        {
            if (keyboard.gameObject.activeSelf)
            {
                keyboard.EnterPressed();
            }
            else
            {
                keyboard.gameObject.SetActive(true);
                keyboard.SetText(textMeshPro.text);
                keyboard.TextChanged += OnChangeText;
                keyboard.EnterPressed += OnEnterPress;
            }
        }

        public string GetText()
        {
            return _text;
        }
        
        public void SetText(string value)
        {
            OnChangeText(value);
        }

        public void OnEnterPress()
        {
            keyboard.TextChanged -= OnChangeText;
            keyboard.EnterPressed -= OnEnterPress;
            keyboard.gameObject.SetActive(false);
        }

        private void OnChangeText(string newText)
        {
            if (newText.Length > maxSymbols)
            {
                newText = newText.Substring(0, maxSymbols);
                keyboard.SetText(newText);
            }

            _text = newText;
            
            if (hide)
            {
                textMeshPro.text = "";
                
                for (var i = 0; i < _text.Length; i++)
                {
                    textMeshPro.text += "*";
                }
            }
            else
            {
                textMeshPro.text = _text;
            }
        }

        private void Update()
        {
            title.SetActive(textMeshPro.text.Length <= 0);
        }
    }
}
