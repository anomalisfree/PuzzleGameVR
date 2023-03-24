using System.Collections;
using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.Views;
using Main.Scripts.VR.Other;
using Main.Scripts.VR.UI;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class PuzzleController : BaseController
    {
        [SerializeField] private PuzzleView puzzleView;
        
        private PuzzleView _puzzleView;
        
        public void Init()
        {
            _puzzleView = FindObjectOfType<PuzzleView>();

            if (_puzzleView == null)
            {
                var options = new Realtime.InstantiateOptions
                {
                    destroyWhenOwnerLeaves = false,
                    destroyWhenLastClientLeaves = true
                };

                _puzzleView = Realtime
                    .Instantiate(puzzleView.gameObject.name, options)
                    .GetComponent<PuzzleView>();
                
                _puzzleView.StartNewPuzzle();
            }

            _puzzleView.Done += PuzzleDone;
        }

        private void PuzzleDone()
        {
            var framePivot = FindObjectOfType<FramePivot>();
            
            if (framePivot != null)
            {
                framePivot.EndPuzzle();
            }
            
            Realtime.Destroy(_puzzleView.gameObject);
            StartCoroutine(StartNextPuzzle());
        }

        private IEnumerator StartNextPuzzle()
        {
            yield return new WaitForSeconds(8f);
            ClientBase.Instance.GetController<LevelController>().StartNextPuzzle();
            Init();
            FindObjectOfType<Watch>().CloseWatch();
        }
    }
}
