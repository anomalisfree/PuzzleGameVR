using System.Collections;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Views;
using UnityEngine;

namespace Main.Scripts.VR.UI
{
    public class FramePivot : MonoBehaviour
    {
        [SerializeField] private List<Transform> levelButtons;
        [SerializeField] private GameObject endFrame;
        [SerializeField] private GameObject connectionDisplay;
        [SerializeField] private GameObject top;

        private LevelView _levelView;

        public void PressBtnUp()
        {
            if (_levelView == null)
            {
                _levelView = FindObjectOfType<LevelView>();
            }
            else
            {
                _levelView.FrameUp();
            }
        }

        public void PressBtnDown()
        {
            if (_levelView == null)
            {
                _levelView = FindObjectOfType<LevelView>();
            }
            else
            {
                _levelView.FrameDown();
            }
        }

        public void EndPuzzle()
        {
            StartCoroutine(EndPuzzleCor());
        }

        private IEnumerator EndPuzzleCor()
        {
            endFrame.SetActive(true);
            yield return new WaitForSeconds(10f);
            endFrame.SetActive(false);
        }

        private void Update()
        {
            if (Camera.main != null)
            {
                foreach (var levelButton in levelButtons)
                {
                    levelButton.LookAt(Camera.main.transform.position);
                }
            }

            if (_levelView == null)
            {
                _levelView = FindObjectOfType<LevelView>();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x, _levelView.GetFrameHeight(), transform.position.z),
                    Time.deltaTime);
            }
        }

        public void SetConnection(bool isConnected)
        {
            connectionDisplay.SetActive(!isConnected);
            top.SetActive(isConnected);
        }
    }
}