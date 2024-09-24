using InputField = TMPro.TMP_InputField;

namespace ClockEngine
{
    public interface IHandMode : IEnable { }
    
    public class NormalMode : IHandMode
    {
        private readonly ITweener tweener;

        public NormalMode(ITweener tweener)
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

    public class EditMode : IHandMode
    {
        private readonly HandDragger dragger;
        private readonly InputField inputField;

        public EditMode(HandDragger dragger, InputField inputField)
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