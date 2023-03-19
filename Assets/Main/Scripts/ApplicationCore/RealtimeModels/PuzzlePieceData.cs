using Autohand;
using Main.Scripts.VR.UI;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.RealtimeModels
{
    public class PuzzlePieceData : RealtimeComponent<PuzzlePieceDataModel>
    {
        private Frame _frame;
        private Frame _currentFrame;
        private Transform _currentFramePivot;
        private Transform _startParent;

        public void Init(int num)
        {
            GetComponent<GrabbableBase>().body = GetComponent<Rigidbody>();
            _startParent = transform.parent;
            model.num = num;
            model.isCorrect = false;
        }

        private void Update()
        {
            if (realtimeView.isOwnedLocallySelf)
                if (model.inFrame)
                {
                    Transform transformThis;
                    (transformThis = transform).position = Vector3.MoveTowards(transform.position,
                        _currentFramePivot.position,
                        Time.deltaTime * 5);

                    var y = transformThis.rotation.eulerAngles.y;

                    while (y < 0)
                    {
                        y += 360;
                    }

                    while (y > 360)
                    {
                        y -= 360;
                    }

                    switch (y)
                    {
                        case > 315:
                        case <= 45:
                            y = 0;
                            break;
                        case > 45 and <= 135:
                            y = 90;
                            break;
                        case > 135 and <= 225:
                            y = 180;
                            break;
                        case > 225 and <= 315:
                            y = 270;
                            break;
                    }

                    model.rot = y;

                    var rotationFrame = _currentFramePivot.rotation;
                    transform.rotation = Quaternion.Lerp(transform.rotation,
                        Quaternion.Euler(rotationFrame.eulerAngles.x, model.rot,
                            rotationFrame.eulerAngles.z), Time.deltaTime * 20);

                    if (model.rot == 180 && _currentFrame.num == model.num)
                    {
                        model.isCorrect = true;
                        //Debug.Log("IsCorrect " + model.num);
                    }
                    else
                    {
                        model.isCorrect = false;
                    }
                }
                else
                {
                    model.isCorrect = false;
                }
        }

        private void OnTriggerStay(Collider other)
        {
            _frame = other.gameObject.GetComponent<Frame>();
            
            if (!realtimeView.isOwnedLocallySelf) return;
            
            if (transform.parent == _startParent)
            {
                if (_frame == null || _currentFrame != null || _frame.hasImage) return;
                
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                model.inFrame = true;
                _frame.hasImage = true;
                _currentFrame = _frame;
                _currentFramePivot = _frame.GetImagePivot();
            }
            else
            {
                GetComponent<Rigidbody>().isKinematic = false;
                model.inFrame = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _frame = other.gameObject.GetComponent<Frame>();

            if (_frame == null || _frame != _currentFrame) return;
            
            _frame.hasImage = false;
            model.inFrame = false;
            _currentFrame = null;
        }
    }
}