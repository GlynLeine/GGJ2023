using GLXEngine;
using GLXEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GLX_Engine
{
    public abstract class Behaviour
    {
        private const BindingFlags m_messageBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.IgnoreReturn;

        private bool m_enabled = true;
        private GameObject m_gameObject;

        public bool enabled
        {
            get { return m_enabled; }
            set
            {
                if (m_gameObject.enabled)
                {
                    if (!m_enabled && value)
                    {
                        ReceiveMessage("OnEnable");
                    }
                    else if (m_enabled && !value)
                    {
                        ReceiveMessage("OnDisable");
                    }
                }
                m_enabled = value;
            }
        }

        public void ReceiveMessage(string methodName, object[] args = null)
        {
            if (args == null) { args = new object[0]; }
            Type type = GetType();
            type.InvokeMember(methodName, m_messageBindingFlags, Type.DefaultBinder, this, args);
        }

        public GameObject gameObject { get { return m_gameObject; } internal set { m_gameObject = value; } }

        public virtual void Update() { }
        public virtual void Render(GLContext glContext) { }
    }
}
