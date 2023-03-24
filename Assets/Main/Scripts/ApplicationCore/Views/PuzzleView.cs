using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.ApplicationCore.RealtimeModels;
using Main.Scripts.VR.UI;
using Normal.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Scripts.ApplicationCore.Views
{
    public class PuzzleView : MonoBehaviour
    {
        [SerializeField] private List<PuzzlePieceData> puzzlePieces;
        [SerializeField] private GameObject framePrefab;
        [SerializeField] private Vector3 startFrameDelta;

        [SerializeField] private List<Vector3> startPoints;

        public Action Done;


        private FramePivot _framePivot;

        private const float FrameSize = 0.2f;
        private const float CheckingTime = 1f;

        private void Start()
        {
            _framePivot = FindObjectOfType<FramePivot>();

            if (_framePivot == null) return;

            var j = 0;

            for (var i = 0; i < puzzlePieces.Count; i++)
            {
                puzzlePieces[i].Init(i);

                if (i % Mathf.Sqrt(puzzlePieces.Count) == 0)
                {
                    j++;
                }

                var frame = Instantiate(framePrefab, _framePivot.transform);

                frame.transform.localPosition =
                    new Vector3((i - j * Mathf.Sqrt(puzzlePieces.Count)) * FrameSize, 0, -j * FrameSize) +
                    startFrameDelta;

                frame.GetComponent<Frame>().num = i;
            }

            StartCoroutine(CheckPuzzle());

            //StartCoroutine(CheckFinish());
        }


        private IEnumerator CheckFinish()
        {
            yield return new WaitForSeconds(10f);
            
            foreach (var puzzlePiece in puzzlePieces)
            {
                puzzlePiece.GetComponent<RealtimeView>().RequestOwnership();
                puzzlePiece.SetTestCorrect();
            }
        }

        public void StartNewPuzzle()
        {
            foreach (var puzzlePiece in puzzlePieces)
            {
                var pointNum = Random.Range(0, startPoints.Count);

                puzzlePiece.transform.position = startPoints[pointNum] +
                                                 new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(1f, 2f), Random.Range(-0.3f, 0.3f));
                
                puzzlePiece.GetComponent<RealtimeView>().RequestOwnership();
                puzzlePiece.GetComponent<RealtimeTransform>().RequestOwnership();
            }
        }

        private IEnumerator CheckPuzzle()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                var correctPuzzles = puzzlePieces.Count(puzzlePiece => puzzlePiece.IsCorrect());
                
                if (correctPuzzles == puzzlePieces.Count)
                {
                    Done?.Invoke();
                }
            }
        }
    }
}