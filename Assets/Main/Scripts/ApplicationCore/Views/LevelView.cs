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
            SetImage(levelData.GetImageNum());
        }

        public void SetNextImage()
        {
            levelData.SetImageNum(levelData.GetImageNum() + 1);

            if (levelData.GetImageNum() < images.Count)
            {
                SetImage(levelData.GetImageNum());
            }
            else
            {
                WinTheGame();
            }
        }

        public void SetImage(int num)
        {
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

        private void WinTheGame()
        {
            Debug.Log("You win a game!");
        }
    }
}