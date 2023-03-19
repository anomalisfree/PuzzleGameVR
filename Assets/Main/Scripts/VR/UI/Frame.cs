using UnityEngine;

namespace Main.Scripts.VR.UI
{
    public class Frame : MonoBehaviour
    {
        [SerializeField] private Transform imagePivot;
        public int num;

        public bool hasImage;
        
        public Transform GetImagePivot()
        {
            return imagePivot;
        }
    }
}
