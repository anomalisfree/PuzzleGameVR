using Main.Scripts.ApplicationCore.Views;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class TimelineTimerController : BaseController
    {
        [SerializeField] private TimelineTimerView timelineTimerView;
        
        private TimelineTimerView _timelineTimerView;
        
        public void Init()
        {
            _timelineTimerView = FindObjectOfType<TimelineTimerView>();

            if (_timelineTimerView == null)
            {
                var options = new Realtime.InstantiateOptions
                {
                    destroyWhenOwnerLeaves = false,
                    destroyWhenLastClientLeaves = false
                };

                _timelineTimerView = Realtime
                    .Instantiate(timelineTimerView.gameObject.name, options)
                    .GetComponent<TimelineTimerView>();
                
                _timelineTimerView.Play();
            }
        }
    }
}
