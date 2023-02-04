using GLXEngine;
using GLXEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLX_Engine
{
    public abstract class Behaviour
    {
        private GameObject m_gameObject;

        public GameObject gameObject { get { return m_gameObject; } }

        public abstract void Update();
        public virtual void Render(GLContext glContext) { }
    }
}
