using UnityEngine;

namespace Main.Scripts.Scenario
{
    public class CollisionTriggerDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var trigger = other.gameObject.GetComponent<CollisionTrigger>();
            
            if (trigger != null)
            {
                trigger.onCollisionEnter.Invoke();
            }
        }
    }
}