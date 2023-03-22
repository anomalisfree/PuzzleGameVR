using Main.Scripts.ApplicationCore.RealtimeModels;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;

        private const float HeightStep = 0.3f;

        public void FrameUp()
        {
            levelData.SetHeight(levelData.GetHeight() + HeightStep);
        }
        
        public void FrameDown()
        {
            levelData.SetHeight(levelData.GetHeight() - HeightStep);
        }

        public float GetFrameHeight()
        {
            return levelData.GetHeight();
        }
    }
}