using System.Collections;
using Normal.Realtime;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class RealtimeMultiplayerView : MonoBehaviour
    {
        [SerializeField] private Realtime realtime;

        public void ConnectToRoom(string roomName)
        {
            StartCoroutine(ConnectToRoomCor(roomName));
        }

        private IEnumerator ConnectToRoomCor(string roomName)
        {
            realtime.Disconnect();
            
            while (realtime.connected)
            {
                yield return new WaitForEndOfFrame();
            }
            
            realtime.Connect(roomName);
        }

        public string GetRoomName()
        {
            return realtime.room.name;
        }

        public void DisconnectFromRoom()
        {
            realtime.Disconnect();
        }
    }
}
