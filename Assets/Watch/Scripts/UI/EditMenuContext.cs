using UnityEngine;
using UnityEngine.UI;

namespace ClockEngine
{
    public class EditMenuContext : MonoBehaviour
    {
        [SerializeField]
        private Button m_editButton, m_applyButton, m_revertButton;

        public void Initialize(IEditor editor)
        {
            this.m_editButton.onClick.AddListener(editor.Enable);
            this.m_editButton.onClick.AddListener(EnableEditMode);

            this.m_applyButton.onClick.AddListener(editor.ApplyChanges);
            this.m_applyButton.onClick.AddListener(DisableEditMode);

            this.m_revertButton.onClick.AddListener(editor.RevertChanges);
            this.m_revertButton.onClick.AddListener(DisableEditMode);
        }

        private void EnableEditMode()
        {
            SetEditMode(true);
        }

        private void DisableEditMode()
        {
            SetEditMode(false);
        }

        private void SetEditMode(bool isEnabled)
        {
            this.m_editButton.gameObject.SetActive(!isEnabled);
            this.m_applyButton.gameObject.SetActive(isEnabled);
            this.m_revertButton.gameObject.SetActive(isEnabled);
        }
    }
}