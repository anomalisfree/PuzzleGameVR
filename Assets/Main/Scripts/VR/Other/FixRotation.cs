using UnityEngine;

namespace Main.Scripts.VR.Other
{
    public class FixRotation : MonoBehaviour
    {
        private float _startAngle;
        
        private void Start()
        {
            _startAngle = transform.localRotation.y;
        }

        private void LateUpdate()
        {
            var localRotation = transform.localRotation;
            localRotation = Quaternion.Euler(localRotation.eulerAngles.x, _startAngle, localRotation.eulerAngles.z);
            transform.localRotation = localRotation;
        }
    }
}
