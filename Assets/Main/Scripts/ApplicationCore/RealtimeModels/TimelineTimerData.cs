using Normal.Realtime;

namespace Main.Scripts.ApplicationCore.RealtimeModels
{
    public class TimelineTimerData : RealtimeComponent<TimelineTimerModel>
    {
        public float Time
        {
            get
            {
                if (model == null) return 0.0f;
                if (model.startTime == 0.0) return 0.0f;
                return (float)(realtime.room.time - model.startTime);
            }
        }

        public bool IsPlaying => model is { isPlaying: true };

        public void Play()
        {
            model.isPlaying = !model.isPlaying;
            model.startTime = realtime.room.time;
        }

        public void SetTime(float timeToSet)
        {
            model.startTime = realtime.room.time - timeToSet;
        }
    }
}