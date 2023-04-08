using Main.Scripts.ApplicationCore.Views;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class LevelController : BaseController
    {
        [SerializeField] private LevelView levelView;
        
        private LevelView _levelView;
        
        public void Init()
        {
            _levelView = FindObjectOfType<LevelView>();

            if (_levelView == null)
            {
                var options = new Realtime.InstantiateOptions
                {
                    destroyWhenOwnerLeaves = false,
                    destroyWhenLastClientLeaves = false
                };

                _levelView = Realtime
                    .Instantiate(levelView.gameObject.name, options)
                    .GetComponent<LevelView>();
            }
            
            _levelView.Init();
        }

        public void StartNextPuzzle()
        {
            _levelView.SetNextImage();
        }

        public void PuzzleDone()
        {
            _levelView.SetPuzzleDone(true);
        }

        public bool GetPuzzleDone()
        {
            return _levelView.GetPuzzleDone();
        }
        
        public void PuzzleNew()
        {
            _levelView.SetPuzzleDone(false);
        }

        public bool GetWinGame()
        {
            return _levelView.GetWinGame();
        }
    }
}
