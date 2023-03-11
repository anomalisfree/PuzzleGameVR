using Main.Scripts.ApplicationCore.Views;
using UnityEngine;

namespace Main.Scripts.ApplicationCore.Controllers
{
    public class LoginController : BaseController
    {
        [SerializeField] private LoginView loginView;
        
        private LoginView _loginView;
        
        public void Init()
        {
            _loginView = Instantiate(loginView);
        }
    }
}
