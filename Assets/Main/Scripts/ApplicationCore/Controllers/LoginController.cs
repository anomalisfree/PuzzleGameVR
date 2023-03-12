using System;
using Main.Scripts.ApplicationCore.Views;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class LoginController : BaseController
    {
        [SerializeField] private LoginView loginView;
        public Action<string, string> IsLogin;
        
        private LoginView _loginView;
        
        public void Init()
        {
            _loginView = Instantiate(loginView);
            _loginView.OnConnect = OnConnect;
        }

        private void OnConnect(string playerName, string room)
        {
            IsLogin?.Invoke(playerName, room);
            Destroy(_loginView.gameObject);
        }
    }
}
