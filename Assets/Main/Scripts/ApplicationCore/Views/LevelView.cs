using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.Controllers;
using Main.Scripts.ApplicationCore.RealtimeModels;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private List<Texture> images;
        [SerializeField] private List<Sprite> previews;
        [SerializeField] private Material imageMaterial;
        [SerializeField] private RealtimeView realtimeView;

        private const float HeightStep = 0.3f;


        public void Init()
        {
            SetImage(levelData.GetImageNum());
            ClientBase.Instance.GetController<PuzzleController>().Init(levelData.GetImageNum());
        }

        public void SetNextImage()
        {
            realtimeView.RequestOwnership();
            levelData.SetImageNum(levelData.GetImageNum() + 1);

            if (levelData.GetImageNum() < images.Count - 1)
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
            ClientBase.Instance.GetController<PuzzleController>().SetCurrentImageNum(num);
        }

        public Sprite GetImagePreview()
        {
            return previews[levelData.GetImageNum()];
        }

        public void FrameUp()
        {
            realtimeView.RequestOwnership();
            levelData.SetHeight(levelData.GetHeight() + HeightStep);
            ClientBase.Instance.GetController<PuzzleController>().RequestOwnershipOnFrame();
        }

        public void FrameDown()
        {
            realtimeView.RequestOwnership();
            levelData.SetHeight(levelData.GetHeight() - HeightStep);
            ClientBase.Instance.GetController<PuzzleController>().RequestOwnershipOnFrame();
        }

        public float GetFrameHeight()
        {
            return levelData.GetHeight();
        }

        private void WinTheGame()
        {
            Debug.Log("You win a game!");
            ClientBase.Instance.GetController<PuzzleController>().isWon = true;
            levelData.SetImageNum(0);
        }
    }
}