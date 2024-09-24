using UnityEngine;

namespace ClockEngine
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private ClockInstaller m_clockInstaller;

        [SerializeField]
        private ServerRunner m_serverRunner;

        private void Start()
        {
            var context = this.m_clockInstaller.Install();
            this.m_serverRunner.Initialize(context);
        }
    }
}
