using InputField = TMPro.TMP_InputField;

namespace ClockEngine
{
    public interface IHandMode : IEnable { }
    
    public class NormalEnable : IHandMode
    {
        private readonly ITweener tweener;

        public NormalEnable(ITweener tweener)
        {
            this.tweener = tweener;
        }

        public void Enable()
        {
            this.tweener.Play();
        }

        public void Disable()
        {
            this.tweener.Pause();
        }
    }

    public class EditEnable : IHandMode
    {
        private readonly HandDragger dragger;
        private readonly InputField inputField;

        public EditEnable(HandDragger dragger, InputField inputField)
        {
            this.dragger = dragger;
            this.inputField = inputField;
        }

        public void Enable()
        {
            this.dragger.enabled = true;
            this.inputField.enabled = true;
        }

        public void Disable()
        {
            this.dragger.enabled = false;
            this.inputField.enabled = false;
        }
    }
}