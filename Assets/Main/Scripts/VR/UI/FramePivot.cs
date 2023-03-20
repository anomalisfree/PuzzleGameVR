using UnityEngine;

namespace Main.Scripts.VR.UI
{
    public class FramePivot : MonoBehaviour
    {
        [SerializeField] private Transform levelButtons;
        
        public void PressBtnUp()
        {
            Debug.Log("PressUpBtn");
        }
        
        public void PressBtnDown()
        {
            Debug.Log("PressDownBtn");
        }

        private void Update()
        {
            if (Camera.main != null) levelButtons.LookAt(Camera.main.transform.position);
        }
    }
}
