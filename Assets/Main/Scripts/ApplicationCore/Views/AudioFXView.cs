using UnityEngine;

namespace Main.Scripts.ApplicationCore.Views
{
    public class AudioFXView : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        public void Init(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(gameObject, clip.length);
        }
    }
}
