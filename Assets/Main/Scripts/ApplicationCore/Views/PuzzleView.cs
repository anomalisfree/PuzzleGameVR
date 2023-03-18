using System.Collections.Generic;
using Main.Scripts.ApplicationCore.RealtimeModels;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class PuzzleView : MonoBehaviour
    {
        [SerializeField] private List<PuzzlePieceData> puzzlePieces;
        [SerializeField] private GameObject framePrefab;

        private const float FrameSize = 0.2f;

        private void Start()
        {
            var j = 0;

            for (var i = 0; i < puzzlePieces.Count; i++)
            {
                puzzlePieces[i].Init(i);

                if (i % Mathf.Sqrt(puzzlePieces.Count) == 0)
                {
                    j--;
                }

                Instantiate(framePrefab,
                    new Vector3((-i - j * Mathf.Sqrt(puzzlePieces.Count))* FrameSize, 0, j * FrameSize),
                    Quaternion.identity);
            }
        }
    }
}