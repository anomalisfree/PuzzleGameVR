using Normal.Realtime;

namespace Main.Scripts.ApplicationCore.RealtimeModels
{
    public class InteractableObjectsData : RealtimeComponent<InteractableObjectsDataModel>
    {
        public void SetIsGrabbed(bool value)
        {
            model.isGrabbed = value;
        }

        public bool GetIsGrabbed()
        {
            return model.isGrabbed;
        }

        public void SetReleaseTimer(float value)
        {
            model.timeAfterRelease = value;
        }

        public float GetTimerValue()
        {
            return model.timeAfterRelease;
        }

        public void AddTimeToTimer(float delta)
        {
            model.timeAfterRelease += delta;
        }
    }
}
