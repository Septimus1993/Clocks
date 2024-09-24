using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ClockEngine
{
    public static class EventTriggerExtensions
    {
        public static void AddCallback(this EventTrigger eventTrigger, EventTriggerType eventID, UnityAction<BaseEventData> callback)
        {
            var triggers = eventTrigger.triggers;
            var triggerEvent = new EventTrigger.TriggerEvent();
            
            triggerEvent.AddListener(callback);
            triggers.Add(new EventTrigger.Entry { eventID = eventID, callback = triggerEvent });
        }

        public static void Clear(this EventTrigger eventTrigger)
        {
            eventTrigger.triggers.Clear();
        }
    }
}