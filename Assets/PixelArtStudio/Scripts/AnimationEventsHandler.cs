
using UnityEngine;

namespace PixelArtStudio.Scripts
{
    public class AnimationEventsHandler : MonoBehaviour
    {
        public void AnimationEventHandler(string eventName)
        {
            Debug.Log($"Event triggered: {eventName}");
        }
    }
}
