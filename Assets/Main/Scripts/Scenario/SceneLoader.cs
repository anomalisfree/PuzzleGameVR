using Main.Scripts.ApplicationCore.Clients;
using UnityEngine;

namespace Main.Scripts.Scenario
{
    public class SceneLoader : MonoBehaviour
    {
        public void ApplicationQuit()
        {
            Debug.Log("Application quit");
            Application.Quit();
        }
        
        public void LoadScene(string sceneName)
        {
            ClientBase.Instance.LoadNewScene(sceneName);
        }

        public void LoadSceneFromBundle(string sceneName)
        {
            ClientBase.Instance.LoadNewScene(sceneName, true);
        }
        
        public void LoadSceneOnline(string sceneName)
        {
            ClientBase.Instance.LoadNewScene(sceneName, sceneName);
        }
        
        public void LoadSceneOnline(string sceneName, string roomName)
        {
            ClientBase.Instance.LoadNewScene(sceneName, roomName);
        }

        public void LoadSceneFromBundleOnline(string sceneName)
        {
            ClientBase.Instance.LoadNewScene(sceneName, sceneName, true);
        }
    }
}
