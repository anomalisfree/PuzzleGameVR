using System;
using System.Collections;
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

        private void Start()
        {
            StartCoroutine(CheckImage());
            StartCoroutine(CheckWinGame());
        }

        public void SetNextImage()
        {
            realtimeView.RequestOwnership();
            levelData.SetImageNum(levelData.GetImageNum() + 1);

            if (levelData.GetImageNum() >= images.Count)
            {
                SetWinGame(true);
                realtimeView.RequestOwnership();
                levelData.SetImageNum(0);
            }
        }
        
        private IEnumerator CheckWinGame()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                if (GetWinGame())
                {
                    WinTheGame();
                    yield return new WaitForSeconds(30f);
                    SetWinGame(false);
                }
            }
        }
        
        private IEnumerator CheckImage()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log(ClientBase.Instance.GetController<PuzzleController>().GetCurrentImageNum() + " : " + levelData.GetImageNum());
                if (ClientBase.Instance.GetController<PuzzleController>().GetCurrentImageNum()!= levelData.GetImageNum())
                {
                    SetImage(levelData.GetImageNum());
                }
            }
        }

        public void SetImage(int num)
        {
            Debug.Log("SetImage " + num);

            if (num < images.Count)
            {
                imageMaterial.mainTexture = images[num];
                ClientBase.Instance.GetController<PuzzleController>().SetCurrentImageNum(num);
            }
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
            ClientBase.Instance.GetController<PuzzleController>().isWon?.Invoke();
        }

        public void SetPuzzleDone(bool value)
        {
            realtimeView.RequestOwnership();
            levelData.SetPuzzleDone(value);
        }

        public bool GetPuzzleDone()
        {
            return levelData.GetPuzzleDone();
        }

        public void SetWinGame(bool value)
        {
            realtimeView.RequestOwnership();
            levelData.SetWinGame(value);
        }

        public bool GetWinGame()
        {
           return levelData.GetWinGame();
        }
    }
}