using System;
using System.Collections.Generic;
using Main.Scripts.ApplicationCore.Clients;
using Main.Scripts.ApplicationCore.Views;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class VrPlayerController : BaseController
    {
        [SerializeField] private VrPlayerView vrPlayerView;

        public Action<Transform, (Transform leftHandRoot, Transform rightHandRoot), List<Transform>> Ready;

        private VrPlayerView _vrPlayerView;

        public void Init()
        {
            _vrPlayerView = Instantiate(vrPlayerView);
            Ready?.Invoke(_vrPlayerView.GetBodyRoot(), _vrPlayerView.GetHandRoots(), _vrPlayerView.GetVrikPoints());
        }

        public void ResetPosition()
        {
            if (_vrPlayerView != null)
            {
                _vrPlayerView.ResetPose();
            }
        }
    }
}