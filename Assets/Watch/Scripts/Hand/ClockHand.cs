using DG.Tweening;
using UnityEngine;
using InputField = TMPro.TMP_InputField;

namespace ClockEngine
{
    public interface IClockHand
    {
        double time { get; }

        void Initialize(ITime clock);
        void Display();
        void Hide();
        void SetNormalMode();
        void SetEditMode();
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

    public interface ITweener
    {
        void Play();
        void Pause();
    }

    public class ClockHand : IClockHand, ITweenHand, ITweener
    {
        private ITime clock;

        private readonly GameObject gameObject;
        private readonly HandDragger dragger;
        private IInitialize inputFacade;

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

        private IHandMode normalMode;
        private IHandMode editMode;
        private IHandMode currentMode;

        public ClockHand(HandDragger dragger, InputField inputField, CalculateTimeFunction calculateTimeFunc,
            double duration, double clampTime, double targetAngle)
        {
            this.duration = duration;
            this.clampTime = clampTime;
            this.targetAngle = targetAngle;

            this.gameObject = dragger.gameObject;
            this.dragger = dragger;
            this.calculateTimeFunc = calculateTimeFunc;

            this.tween = this.gameObject.transform.DORotate(Vector3.back * (float) this.targetAngle, (float) this.duration, RotateMode.LocalAxisAdd)
                .SetLoops(-1)
                .Pause();

            this.normalMode = new NormalEnable(this);
            this.editMode = new EditEnable(dragger, inputField);
            this.inputFacade = new InputFacade(this, inputField);
        }

        public void Initialize(ITime clock)
        {
            this.clock = clock;
            this.inputFacade.Initialize();
            this.dragger.onAngleChanged += AddAngle;
        }

        public void Display()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Play()
        {
            this.tween.Play();
        }

        public void Pause()
        {
            this.tween.Pause();
        }

        public void SetNormalMode()
        {
            SetMode(this.normalMode);
        }

        public void SetEditMode()
        {
            SetMode(this.editMode);
        }

        private void SetMode(IHandMode mode)
        {
            this.currentMode?.Disable();
            this.currentMode = mode;
            this.currentMode?.Enable();
        }

        private void AddAngle(double deltaAngle)
        {
            var deltaTime = deltaAngle * (this.duration / this.targetAngle);

            AddTime(deltaTime);
        }

        public void AddTime(double deltaTime)
        {
            this.clock.AddTime(deltaTime, false);
        }

        public void GoTo(double totalTime, bool play)
        {
            if (play != (this.currentMode == this.normalMode))
                return;

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