
using UnityEngine;

namespace CommonSoldier.Scripts
{
    public class AnimationEventsHandler : MonoBehaviour
    {
        public void AnimationEventHandler(string eventName)
        {
            Debug.Log($"Event triggered: {eventName}");
        }
    }
}
