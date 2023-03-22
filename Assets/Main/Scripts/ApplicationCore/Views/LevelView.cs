using System.Collections.Generic;
using Main.Scripts.ApplicationCore.RealtimeModels;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private List<Texture> images;
        [SerializeField] private List<Sprite> previews;
        [SerializeField] private Material imageMaterial;

        private int _currentImageNum;

        private const float HeightStep = 0.3f;


        public void Init()
        {
            SetNewImage(0);
        }

        public void UpdateImageMaterial()
        {
            SetNewImage(_currentImageNum);
        }
        
        public void SetNewImage(int num)
        {
            _currentImageNum = num;
            imageMaterial.mainTexture = images[num];
        }

        public Sprite GetImagePreview()
        {
            return previews[_currentImageNum];
        }
        
        public void FrameUp()
        {
            levelData.SetHeight(levelData.GetHeight() + HeightStep);
        }
        
        public void FrameDown()
        {
            levelData.SetHeight(levelData.GetHeight() - HeightStep);
        }

        public float GetFrameHeight()
        {
            return levelData.GetHeight();
        }
    }
}