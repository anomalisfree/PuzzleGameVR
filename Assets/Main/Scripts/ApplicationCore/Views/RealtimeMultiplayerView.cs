using System;
using System.Collections;
using Main.Scripts.VR.UI;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class RealtimeMultiplayerView : MonoBehaviour
    {
        [SerializeField] private Realtime realtime;

        private string _currentRoom;
        
        public void ConnectToRoom(string roomName)
        {
            _currentRoom = roomName;
            StartCoroutine(ConnectToRoomCor(roomName));
        }

        private IEnumerator ConnectToRoomCor(string roomName)
        {
            realtime.Disconnect();
            
            realtime.didConnectToRoom += DidConnectToRoom;
            realtime.didDisconnectFromRoom += DidDisconnectFromRoom;

            while (realtime.connected)
            {
                yield return new WaitForEndOfFrame();
            }
            
            realtime.Connect(roomName);
        }

        private void DidDisconnectFromRoom(Realtime realtime1)
        {
            realtime.didDisconnectFromRoom -= DidDisconnectFromRoom;
            FindObjectOfType<FramePivot>().SetConnection(false);
            ConnectToRoom(_currentRoom);
        }

        private void DidConnectToRoom(Realtime realtime1)
        {
            realtime.didConnectToRoom -= DidConnectToRoom;
            FindObjectOfType<FramePivot>().SetConnection(true);
        }

        public string GetRoomName()
        {
            return realtime.room.name;
        }

        public void DisconnectFromRoom()
        {
            realtime.Disconnect();
        }

        private void OnDestroy()
        {
            realtime.didConnectToRoom -= DidConnectToRoom;
            realtime.didDisconnectFromRoom -= DidDisconnectFromRoom;
        }
    }
}
