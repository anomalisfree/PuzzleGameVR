using System.Collections;
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

        private float _leftHandTriggerValue;
        private float _rightHandTriggerValue;

        private const float HeightDelta = -0.15f;

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
            if (!XRGeneralSettings.Instance.Manager.isInitializationComplete) return;
            if (leftHandTrigger == null || rightHandTrigger == null) return;

            setImage.rectTransform.sizeDelta = new Vector2(setImage.rectTransform.sizeDelta.x,
                (_leftHandTriggerValue + _rightHandTriggerValue) *
                setImage.transform.parent.GetComponent<RectTransform>().sizeDelta.y / 2);

            if (!((_leftHandTriggerValue + _rightHandTriggerValue) > 1.9f)) return;
            
            PlayerPrefs.SetFloat("HeadDefaultHeight", head.position.y + HeightDelta);
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
}