using System;
using Main.Scripts.ApplicationCore.Data;
using Main.Scripts.ApplicationCore.Views;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class LoginController : BaseController
    {
        [SerializeField] private LoginView loginView;
        public Action<string, string, Gender> IsLogin;
        
        private LoginView _loginView;
        
        public void Init()
        {
            _loginView = Instantiate(loginView);
            _loginView.OnConnect = OnConnect;
        }

        private void OnConnect(string playerName, string room, Gender gender)
        {
            IsLogin?.Invoke(playerName, room, gender);
            Destroy(_loginView.gameObject);
        }
    }
}
