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

        private const float HeightStep = 0.3f;


        public void Init()
        {
            SetNewImage(0);
        }

        public void UpdateImageMaterial()
        {
            SetNewImage(levelData.GetImageNum());
        }
        
        public void SetNewImage(int num)
        {
            levelData.SetImageNum(num);
            imageMaterial.mainTexture = images[num];
        }

        public Sprite GetImagePreview()
        {
            return previews[levelData.GetImageNum()];
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