using Main.Scripts.ApplicationCore.RealtimeModels;
using UnityEngine;
using UnityEngine.Playables;

namespace Main.Scripts.ApplicationCore.Views
{
    public class TimelineTimerView : MonoBehaviour
    {
        [SerializeField] private TimelineTimerData timelineTimer;
        public bool repeat;

        private PlayableDirector[] _playableDirectors;
        private double _maxTime;
        private const float ErrorDelta = 0.5f;

        private void Start()
        {
            GetSceneData();
        }

        private void GetSceneData()
        {
            _playableDirectors = FindObjectsOfType<PlayableDirector>();
            
            foreach (var playableDirector in _playableDirectors)
            {
                if (playableDirector.duration > _maxTime) _maxTime = playableDirector.duration;
            }
        }

        private void Update()
        {
            UpdatePlayableDirectors();
        }

        private void UpdatePlayableDirectors()
        {
            foreach (var playableDirector in _playableDirectors)
            {
                if (playableDirector != null)
                {
                    if (repeat)
                    {
                        if (timelineTimer.Time >= _maxTime && timelineTimer.Time >= playableDirector.duration)
                        {
                            playableDirector.Pause();
                            playableDirector.time = 0;
                            playableDirector.Play();
                            timelineTimer.Play();
                            return;
                        }
                    }

                    if (timelineTimer.Time < playableDirector.duration)
                    {
                        if (Mathf.Abs((float)(playableDirector.time - timelineTimer.Time)) > ErrorDelta)
                        {
                            playableDirector.Pause();
                            playableDirector.time = timelineTimer.Time;
                            playableDirector.Play();
                        }
                    }
                    else
                    {
                        playableDirector.Pause();
                        playableDirector.time = 0;
                    }
                }
                else
                {
                    GetSceneData();
                }
            }
        }
        
        public void Play()
        {
            timelineTimer.Play();
        }
    }
}