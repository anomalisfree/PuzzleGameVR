using UnityEngine;
using UnityEngine.UI;

namespace Main.Scripts.ApplicationCore.Views
{
    public class SceneLoaderView : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void SetSliderValue(float value)
        {
            slider.value = value;
        }

        public float GetSliderValue()
        {
            return slider.value;
        }
    }
}
