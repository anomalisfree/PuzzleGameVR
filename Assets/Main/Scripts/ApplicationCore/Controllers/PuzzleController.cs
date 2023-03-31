using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.RealtimeModels;
using Main.Scripts.VR.Other;
using Main.Scripts.VR.UI;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class PuzzleController : BaseController
    {
        [SerializeField] private List<PuzzlePieceData> puzzlePiece4x4;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece5x5;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece6x6;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece7x7;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece8x8;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece9x9;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece10x10;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece11x11;
        [SerializeField] private List<PuzzlePieceData> puzzlePiece12x12;
        
        
        private List<PuzzlePieceData> _puzzlePiecePrefabs;
        [SerializeField] private GameObject framePrefab;

        [SerializeField] private List<Vector3> startPoints;

        private List<PuzzlePieceData> _puzzlePieces = new List<PuzzlePieceData>();
        private FramePivot _framePivot;
        private int _currentImageNum;

        private const float FrameSize = 0.2f;
        private const float CheckingTime = 1f;

        public void Init(int imageNum)
        {
            _currentImageNum = imageNum;

            _puzzlePiecePrefabs = _currentImageNum switch
            {
                0 => puzzlePiece4x4,
                1 => puzzlePiece4x4,
                2 => puzzlePiece4x4,
                3 => puzzlePiece4x4,
                4 => puzzlePiece4x4,
                5 => puzzlePiece4x4,
                6 => puzzlePiece4x4,
                7 => puzzlePiece4x4,
                8 => puzzlePiece4x4,
                9 => puzzlePiece4x4,
                10 => puzzlePiece4x4,
                11 => puzzlePiece4x4,
                12 => puzzlePiece4x4,
                _ => _puzzlePiecePrefabs
            };

            _framePivot = FindObjectOfType<FramePivot>();
            var edgePoint = Mathf.Sqrt(_puzzlePiecePrefabs.Count) * FrameSize / 2 + FrameSize / 2;
            _framePivot.SetFrameSize(edgePoint, _puzzlePiecePrefabs.Count);

            if (_framePivot == null) return;

            var j = 0;

            for (var i = 0; i < _puzzlePiecePrefabs.Count; i++)
            {
                if (i % Mathf.Sqrt(_puzzlePiecePrefabs.Count) == 0)
                {
                    j++;
                }

                var frame = Instantiate(framePrefab, _framePivot.transform);

                frame.transform.localPosition =
                    new Vector3((i - j * Mathf.Sqrt(_puzzlePiecePrefabs.Count)) * FrameSize, 0, -j * FrameSize)
                    + new Vector3(1, 0, 1) * edgePoint;

                frame.GetComponent<Frame>().num = i;
            }


            _puzzlePieces.Clear();
            _puzzlePieces.AddRange(FindObjectsOfType<PuzzlePieceData>());
            if (_puzzlePieces.Count != _puzzlePiecePrefabs.Count)
            {
                foreach (var puzzlePiece in _puzzlePieces)
                {
                    Realtime.Destroy(puzzlePiece.gameObject);
                }

                StartNewPuzzle();
            }
            else
            {
                foreach (var puzzlePiece in _puzzlePieces)
                {
                    if (puzzlePiece.GetComponent<RealtimeView>().isUnownedSelf ||
                        puzzlePiece.GetComponent<RealtimeTransform>().isUnownedSelf)
                    {
                        puzzlePiece.GetComponent<RealtimeView>().RequestOwnership();
                        puzzlePiece.GetComponent<RealtimeTransform>().RequestOwnership();
                    }
                }

                RequestOwnershipOnFrame();
            }

            StopAllCoroutines();
            StartCoroutine(CheckPuzzle());
            //StartCoroutine(CheckFinish());
        }


        public void RequestOwnershipOnFrame()
        {
            foreach (var puzzlePiece in _puzzlePieces)
            {
                if (puzzlePiece.IsInFrame())
                {
                    puzzlePiece.GetComponent<RealtimeView>().RequestOwnership();
                    puzzlePiece.GetComponent<RealtimeTransform>().RequestOwnership();
                }
            }
        }

        private void StartNewPuzzle()
        {
            for (var i = 0; i < _puzzlePiecePrefabs.Count; i++)
            {
                var options = new Realtime.InstantiateOptions
                {
                    destroyWhenOwnerLeaves = false,
                    destroyWhenLastClientLeaves = false
                };

                _puzzlePieces.Add(Realtime.Instantiate($"Puzzles/{_puzzlePiecePrefabs[i].gameObject.name}", options)
                    .GetComponent<PuzzlePieceData>());
                _puzzlePieces[i].Init(i);
            }

            foreach (var puzzlePiece in _puzzlePieces)
            {
                var pointNum = Random.Range(0, startPoints.Count);

                puzzlePiece.transform.position = startPoints[pointNum] +
                                                 new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(1f, 2f),
                                                     Random.Range(-0.3f, 0.3f));

                puzzlePiece.GetComponent<RealtimeView>().RequestOwnership();
                puzzlePiece.GetComponent<RealtimeTransform>().RequestOwnership();
            }
        }

        private IEnumerator CheckPuzzle()
        {
            while (true)
            {
                yield return new WaitForSeconds(CheckingTime);
                var correctPuzzles = _puzzlePieces.Count(puzzlePiece => puzzlePiece.IsCorrect());

                if (correctPuzzles == _puzzlePieces.Count && _puzzlePieces.Count > 0)
                {
                    PuzzleDone();
                }
            }
        }

        private IEnumerator CheckFinish()
        {
            yield return new WaitForSeconds(12f);

            foreach (var puzzlePiece in _puzzlePieces)
            {
                puzzlePiece.GetComponent<RealtimeView>().RequestOwnership();
                puzzlePiece.SetTestCorrect();
            }
        }

        private void PuzzleDone()
        {
            var framePivot = FindObjectOfType<FramePivot>();

            if (framePivot != null)
            {
                framePivot.EndPuzzle();
            }

            foreach (var puzzlePiece in _puzzlePieces)
            {
                Realtime.Destroy(puzzlePiece.gameObject);
            }

            _puzzlePieces.Clear();

            foreach (var frame in FindObjectsOfType<Frame>())
            {
                Destroy(frame.gameObject);
            }

            StartCoroutine(StartNextPuzzle());
        }

        private IEnumerator StartNextPuzzle()
        {
            yield return new WaitForSeconds(8f);
            ClientBase.Instance.GetController<LevelController>().StartNextPuzzle();
            Init(_currentImageNum);
            FindObjectOfType<Watch>().CloseWatch();
        }

        public void SetCurrentImageNum(int imageNum)
        {
            _currentImageNum = imageNum;
        }
    }
}