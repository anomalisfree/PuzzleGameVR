using Normal.Realtime;

namespace Main.Scripts.ApplicationCore.RealtimeModels
{
    public class LevelData : RealtimeComponent<LevelDataModel>
    {
        private const float MaxHeight = 1.5f;
        private const float MinHeight = 0.1f;

        public void SetHeight(float height)
        {
            if (height > MaxHeight) height = MaxHeight;
            else if (height < MinHeight) height = MinHeight;
            model.frameHeight = height;
        }

        public float GetHeight()
        {
            return model.frameHeight;
        }

        public void SetImageNum(int num)
        {
            model.imageNum = num;
        }

        public int GetImageNum()
        {
            return model.imageNum;
        }

        public void SetPuzzleDone(bool value)
        {
            model.puzzleDone = value;
        }
        
        public bool GetPuzzleDone()
        {
            return  model.puzzleDone;
        }

        public void SetWinGame(bool value)
        {
            model.winGame = value;
        }

        public bool GetWinGame()
        {
            return model.winGame;
        }
    }
}