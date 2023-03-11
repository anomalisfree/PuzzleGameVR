using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Main.Scripts.VR.UI
{
    public class KeyboardVR : MonoBehaviour
    {
        public Action<string> TextChanged;
        public Action EnterPressed;

        [SerializeField] private List<GameObject> keysWithLetters;
        private string _text;
        private bool _isCapsLock;

        public void AddSymbol(GameObject symbolKey)
        {
            _text += symbolKey.name;
            TextChanged?.Invoke(_text);

            if (!_isCapsLock)
            {
                ToLowerLetters();
            }
        }

        public void Backspace()
        {
            if (_text.Length > 0)
            {
                _text = _text.Substring(0, _text.Length - 1);
            }
            
            TextChanged?.Invoke(_text);
        }

        public void CapsLock()
        {
            _isCapsLock = !_isCapsLock;

            if (_isCapsLock)
            {
                ToUpperLetters();
            }
            else
            {
                ToLowerLetters();
            }
        }

        public void Shift()
        {
            _isCapsLock = false;
            ToUpperLetters();
        }

        public void ClearAll()
        {
            _text = "";
        }

        public void Enter()
        {
            EnterPressed?.Invoke();
        }

        public void SetText(string newText)
        {
            _text = newText;
        }

        private void ToUpperLetters()
        {
            foreach (var key in keysWithLetters)
            {
                key.name = key.name.ToUpper();
                key.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key.name;
            }
        }
        
        private void ToLowerLetters()
        {
            foreach (var key in keysWithLetters)
            {
                key.name = key.name.ToLower();
                key.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key.name;
            }
        }
    }
}
