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
        [SerializeField] private GameObject cup;

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

        public void SetFrameSize(float edgePoint, int puzzleCount)
        {
            levelButtons[0].GetComponent<RectTransform>().localPosition =
                new Vector3(-1, 0, 1) * (edgePoint) + new Vector3(0, 0.1f, 0);
            levelButtons[1].GetComponent<RectTransform>().localPosition =
                new Vector3(1, 0, 1) * (edgePoint) + new Vector3(0, 0.1f, 0);
            levelButtons[2].GetComponent<RectTransform>().localPosition =
                new Vector3(1, 0, -1) * (edgePoint) + new Vector3(0, 0.1f, 0);
            levelButtons[3].GetComponent<RectTransform>().localPosition =
                new Vector3(-1, 0, -1) * (edgePoint) + new Vector3(0, 0.1f, 0);

            endFrame.transform.localScale = Vector3.one / 12f * Mathf.Sqrt(puzzleCount);
            top.GetComponent<RectTransform>().localPosition =  new Vector3(0, 0, 1) * (edgePoint + 0.05f);
        }

        public void Won()
        {
            cup.SetActive(true);
        }
    }
}