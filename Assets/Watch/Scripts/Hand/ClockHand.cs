using DG.Tweening;
using UnityEngine;
using InputField = TMPro.TMP_InputField;

namespace ClockEngine
{
    public delegate float CalculateTimeFunction(double totalTime);

    public class ClockHand
    {
        private readonly GameObject gameObject;

        public readonly Tween tween;

        public double time => this.tween.position;
        public readonly double duration;
        public readonly double clampTime;
        public readonly double targetAngle;

        private TimeAddCallback onTimeAdded;
        private readonly CalculateTimeFunction timeFunc;

        private NormalHandMode normalMode;
        private EditHandMode editMode;
        private InputHandMode inputMode;
        private IHandMode currentMode;

        public ClockHand(HandDragger dragger, InputField inputField, CalculateTimeFunction timeFunc,
            double duration, double clampTime, double targetAngle)
        {
            this.duration = duration;
            this.clampTime = clampTime;
            this.targetAngle = targetAngle;

            this.gameObject = dragger.gameObject;
            this.timeFunc = timeFunc;

            this.tween = this.gameObject.transform.DORotate(Vector3.back * (float) this.targetAngle, (float) this.duration, RotateMode.LocalAxisAdd)
                .SetLoops(-1)
                .Pause();

            this.normalMode = new NormalHandMode(this);
            this.editMode = new EditHandMode(this, dragger, inputField);
            this.inputMode = new InputHandMode(this, inputField);
        }

        public void Initialize(TimeAddCallback callback)
        {
            this.onTimeAdded += callback;
            this.editMode.Initialize();
            this.inputMode.Initialize();
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

        public void AddTime(double deltaTime)
        {
            this.onTimeAdded?.Invoke(deltaTime);
        }

        public void GoTo(double totalTime, bool play)
        {
            this.tween.Goto(CalculateTime(totalTime), play);
        }

        private float CalculateTime(double totalTime)
        {
            return this.timeFunc.Invoke(totalTime);
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