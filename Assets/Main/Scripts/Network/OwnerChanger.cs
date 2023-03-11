using System.Collections;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.RealtimeModels;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.Network
{
    public class OwnerChanger : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayerMask;
        [SerializeField] private List<OwnerChanger> connectedObjects;

        private InteractableObjectsData _interactableObjectSender;
        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;
        
        private float _timer;

        private void Start()
        {
            _interactableObjectSender = GetComponent<InteractableObjectsData>();
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private IEnumerator Timer()
        {
            _timer = 0;

            while (true)
            {
                _interactableObjectSender.AddTimeToTimer(_timer);
                _timer += 0.02f;
                yield return new WaitForSeconds(_timer);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_realtimeView.isOwnedLocallySelf)
            {
                var otherInteractableObjectSender = other.gameObject.GetComponent<InteractableObjectsData>();


                if (playerLayerMask == (playerLayerMask | (1 << other.gameObject.layer)))
                {
                    if (!_interactableObjectSender.GetIsGrabbed())
                    {
                        RequestOwnership();
                    }
                }
                else if (otherInteractableObjectSender != null)
                {
                    if (otherInteractableObjectSender.realtimeView.isOwnedLocallySelf)
                    {
                        if (otherInteractableObjectSender.GetTimerValue() <= _interactableObjectSender.GetTimerValue())
                        {
                            if (!_interactableObjectSender.GetIsGrabbed())
                            {
                                RequestOwnership();
                            }
                        }
                    }
                }
            }
        }

        public void RequestOwnership(bool secondWave = false)
        {
            _realtimeView.RequestOwnership();
            _realtimeTransform.RequestOwnership();
            StopAllCoroutines();
            StartCoroutine(Timer());

            if (secondWave) return;
            
            foreach (var connectedObject in connectedObjects)
            {
                connectedObject.RequestOwnership(true);
            }
        }

        public void SetIsGrabbed(bool isGrabbed)
        {
            _interactableObjectSender.SetIsGrabbed(isGrabbed);
        }
    }
}