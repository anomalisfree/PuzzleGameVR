using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Management;

namespace Main.Scripts.VR.HeightChecker
{
    public class PlayerHeightAdjustment : MonoBehaviour
    {
        [SerializeField] private InputActionReference leftHandTrigger;
        [SerializeField] private InputActionReference rightHandTrigger;

        [SerializeField] private Transform head;
        [SerializeField] private Image setImage;

        [SerializeField] private List<GameObject> objectsToHide;
        [SerializeField] private List<GameObject> objectsToShow;

        private float _leftHandTriggerValue;
        private float _rightHandTriggerValue;

        private const float HeightDelta = -0.15f;

        private bool _isOk;

        private void Start()
        {
            if (!XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                StartCoroutine(LoadInNotVR());
            }
            else
            {
                if (leftHandTrigger == null || rightHandTrigger == null)
                    return;

                leftHandTrigger.action.Enable();
                rightHandTrigger.action.Enable();

                leftHandTrigger.action.performed +=
                    context => _leftHandTriggerValue = context.action.ReadValue<float>();
                rightHandTrigger.action.performed +=
                    context => _rightHandTriggerValue = context.action.ReadValue<float>();
            }
        }


        private static IEnumerator LoadInNotVR()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }

        private void Update()
        {
            if(_isOk) return;
            
            if (!XRGeneralSettings.Instance.Manager.isInitializationComplete) return;
            if (leftHandTrigger == null || rightHandTrigger == null) return;

            setImage.rectTransform.sizeDelta = new Vector2(setImage.rectTransform.sizeDelta.x,
                (_leftHandTriggerValue + _rightHandTriggerValue) *
                setImage.transform.parent.GetComponent<RectTransform>().sizeDelta.y / 2);

            if (!((_leftHandTriggerValue + _rightHandTriggerValue) > 1.9f)) return;
            
            PlayerPrefs.SetFloat("HeadDefaultHeight", head.position.y + HeightDelta);
            _isOk = true;

            StartCoroutine(ShowTitle());
        }

        private IEnumerator ShowTitle()
        {
            foreach (var objectToHide in objectsToHide)
            {
                objectToHide.SetActive(false);
            }
            
            foreach (var objectToShow in objectsToShow)
            {
                objectToShow.SetActive(true);
            }
            
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
}