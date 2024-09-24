using UnityEngine.EventSystems;
using InputField = TMPro.TMP_InputField;

namespace ClockEngine
{
    public class InputFacade : IInitialize
    {
        private readonly ITweenHand hand;

        private double time => this.hand.time;
        private double duration => this.hand.duration;
        private double clampTime => this.hand.clampTime;

        private readonly InputField inputField;
        private readonly EventTrigger eventTrigger;

        public InputFacade(ITweenHand hand, InputField inputField)
        {
            this.hand = hand;
            this.inputField = inputField;
            this.eventTrigger = this.inputField.GetComponent<EventTrigger>();
        }

        public void Initialize()
        {
            this.inputField.onEndEdit.AddListener(OnTextEditEnd);

            this.eventTrigger.Clear();
            this.eventTrigger.AddCallback(EventTriggerType.Select, OnSelect);
            this.eventTrigger.AddCallback(EventTriggerType.Deselect, OnDeselect);

            EnableText();
        }

        public void Enable()
        {
            this.inputField.enabled = true;
        }

        public void Disable()
        {
            this.inputField.enabled = false;
        }

        private void OnSelect(BaseEventData eventData)
        {
            DisableText();
            this.inputField.onValueChanged.AddListener(OnTextEdit);
        }

        private void OnDeselect(BaseEventData eventData)
        {
            this.inputField.onValueChanged.RemoveListener(OnTextEdit);
            EnableText();
        }

        private void EnableText()
        {
            this.hand.onUpdate += UpdateText;
        }

        private void DisableText()
        {
            this.hand.onUpdate -= UpdateText;
        }

        private void UpdateText()
        {
            this.inputField.text = $"{(int)(this.time / this.duration * this.clampTime % this.clampTime):00}";
        }

        private void OnTextEdit(string text)
        {
            var ratio = this.duration / this.clampTime;
            var newTime = int.Parse(text);
            var oldTime = (int)(this.time / ratio);
            var deltaTime = (newTime - oldTime) * ratio;

            this.hand.AddTime(deltaTime);
        }

        private void OnTextEditEnd(string text)
        {
            this.inputField.text = $"{int.Parse(text) % this.clampTime:00}";
        }
    }
}