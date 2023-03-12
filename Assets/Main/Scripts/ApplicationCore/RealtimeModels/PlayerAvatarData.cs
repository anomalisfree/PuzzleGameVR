using Main.Scripts.ApplicationCore.Data;
using Normal.Realtime;

namespace Main.Scripts.ApplicationCore.RealtimeModels
{
    public class PlayerAvatarData : RealtimeComponent<PlayerAvatarDataModel>
    {
        public void SetUsername(string username)
        {
            model.username = username;
        }

        public string GetUsername()
        {
            return model.username;
        }
        
        public void SetHeight(float height)
        {
            model.height = height;
        }

        public float GetHeight()
        {
            return model.height;
        }

        public void SetGender(Gender gender)
        {
            model.gender = (int)gender;
        }

        public Gender GetGender()
        {
            return (Gender)model.gender;
        }
    }
}
