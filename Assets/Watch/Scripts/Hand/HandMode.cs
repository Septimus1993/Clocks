using DG.Tweening;
using UnityEngine.EventSystems;
using InputField = TMPro.TMP_InputField;

namespace ClockEngine
{
    public interface IHandMode
    {
        void Enable();
        void Disable();
    }

    public abstract class BaseHandMode : IHandMode
    {
        protected readonly ClockHand hand;

        protected BaseHandMode(ClockHand hand)
        {
            this.hand = hand;
        }

        public abstract void Enable();

        public abstract void Disable();
    }

    public class NormalHandMode : BaseHandMode
    {
        protected Tween tween => this.hand.tween;

        public NormalHandMode(ClockHand hand) : base(hand) { }

        public override void Enable()
        {
            this.tween.Play();
        }

        public override void Disable()
        {
            this.tween.Pause();
        }
    }

    public class EditHandMode : BaseHandMode
    {
        private readonly HandDragger dragger;
        private readonly InputField inputField;

        private double duration => this.hand.duration;
        private double targetAngle => this.hand.targetAngle;

        public EditHandMode(ClockHand hand, HandDragger dragger, InputField inputField) : base(hand)
        {
            this.dragger = dragger;
            this.inputField = inputField;
        }

        public void Initialize()
        {
            this.dragger.onAngleChanged += UpdateTime;
        }

        public override void Enable()
        {
            this.dragger.enabled = true;
            this.inputField.enabled = true;
        }

        public override void Disable()
        {
            this.dragger.enabled = false;
            this.inputField.enabled = false;
        }

        private void UpdateTime(double deltaAngle)
        {
            var deltaTime = deltaAngle * (this.duration / this.targetAngle);
            this.hand.AddTime(deltaTime);
        }
    }

    public class InputHandMode : BaseHandMode
    {
        private readonly InputField inputField;
        private readonly EventTrigger eventTrigger;

        private event TweenCallback onUpdate
        {
            add => this.hand.tween.onUpdate += value;
            remove => this.hand.tween.onUpdate -= value;
        }

        private double time => this.hand.time;
        private double duration => this.hand.duration;
        private double clampTime => this.hand.clampTime;

        public InputHandMode(ClockHand hand, InputField inputField) : base(hand)
        {
            this.inputField = inputField;
            this.eventTrigger = this.inputField.GetComponent<EventTrigger>();
        }

        public void Initialize()
        {
            this.inputField.onEndEdit.AddListener(OnTextEditEnd);

            this.eventTrigger.Clear();
            this.eventTrigger.AddCallback(EventTriggerType.Select, EnableInputMode);
            this.eventTrigger.AddCallback(EventTriggerType.Deselect, DisableInputMode);

            EnableText();
        }

        public override void Enable()
        {
            this.DisableText();
            this.inputField.onValueChanged.AddListener(OnTextEdit);
        }

        public override void Disable()
        {
            this.inputField.onValueChanged.RemoveListener(OnTextEdit);
            this.EnableText();
        }

        private void EnableInputMode(BaseEventData eventData)
        {
            Enable();
        }

        private void DisableInputMode(BaseEventData eventData)
        {
            Disable();
        }

        public void EnableText()
        {
            this.onUpdate += UpdateText;
        }

        public void DisableText()
        {
            this.onUpdate -= UpdateText;
        }

        private void UpdateText()
        {
            this.inputField.text = $"{(int)(this.time / this.duration * this.clampTime % this.clampTime):00}";
        }

        private void OnTextEdit(string text)
        {
            var ratio = this.duration / this.clampTime;
            var newTime = int.Parse(text);
            var oldTime = (int) (this.time / ratio);
            var deltaTime = (newTime - oldTime) * ratio;

            this.hand.AddTime(deltaTime);
        }

        private void OnTextEditEnd(string text)
        {
            this.inputField.text = $"{int.Parse(text) % this.clampTime:00}";
        }
    }
}