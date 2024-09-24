using DG.Tweening;
using UnityEngine;
using InputField = TMPro.TMP_InputField;

namespace ClockEngine
{
    public interface ITimerHand
    {
        void Initialize(ITimer timer);
        void GoTo(double totalTime, bool play);
    }

    public interface ITweenHand
    {
        double time { get; }
        double duration { get; }
        double clampTime { get; }

        event TweenCallback onUpdate;

        void AddTime(double deltaTime);
    }

    public class ClockHand : ITimerHand, ITweenHand, IAngleDraggable, IDisplay, IEditable
    {
        private readonly GameObject gameObject;

        private ITimer timer;
        private readonly HandDragger dragger;
        private readonly InputFacade input;

        private readonly Tween tween;

        public event TweenCallback onUpdate
        {
            add => this.tween.onUpdate += value;
            remove => this.tween.onUpdate -= value;
        }

        private readonly CalculateTimeFunction calculateTimeFunc;

        public double time => this.tween.position;
        public double duration { get; }
        public double clampTime { get; }
        public double targetAngle { get; }

        public ClockHand(HandDragger dragger, InputField inputField, CalculateTimeFunction calculateTimeFunc,
            double duration, double clampTime, double targetAngle)
        {
            this.duration = duration;
            this.clampTime = clampTime;
            this.targetAngle = targetAngle;

            this.gameObject = dragger.gameObject;
            this.dragger = dragger;
            this.input = new InputFacade(this, inputField);

            this.calculateTimeFunc = calculateTimeFunc;

            this.tween = this.gameObject.transform
                .DORotate(Vector3.back * (float) this.targetAngle, (float) this.duration, RotateMode.LocalAxisAdd)
                .SetLoops(-1)
                .Pause();
        }

        public void Initialize(ITimer timer)
        {
            this.timer = timer;
            this.input.Initialize();
            this.dragger.Initialize(this);

            SetNormalMode();
        }

        public void Display()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void SetNormalMode()
        {
            this.dragger.enabled = false;
            this.input.Disable();
            this.tween.Play();
        }

        public void SetEditMode()
        {
            this.tween.Pause();
            this.dragger.enabled = true;
            this.input.Enable();
        }

        public void AddAngle(double deltaAngle)
        {
            var deltaTime = deltaAngle * (this.duration / this.targetAngle);

            AddTime(deltaTime);
        }

        public void AddTime(double deltaTime)
        {
            this.timer.AddTime(deltaTime, false);
        }

        public void GoTo(double totalTime, bool play)
        {
            var time = this.calculateTimeFunc.Invoke(totalTime);
            this.tween.Goto(time, play);
        }
    }

    [System.Serializable]
    public class HandData
    {
        public HandDragger dragger;
        public InputField inputField;

        public ClockHand ToHand(CalculateTimeFunction calculateFunc,
            double duration, double clampTime, double targetAngle)
        {
            return new ClockHand(this.dragger, this.inputField, calculateFunc, duration, clampTime, targetAngle);
        }
    }
}