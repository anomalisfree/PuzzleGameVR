using Main.Scripts.ApplicationCore.Views;
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
            }
        }
    }
}