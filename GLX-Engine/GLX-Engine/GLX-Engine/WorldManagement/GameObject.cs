using System;
using System.Collections.Generic;
using System.Reflection;
using GLX_Engine;
using GLXEngine.Core;

namespace GLXEngine
{
    public sealed class GameObject : Transformable
    {
        public string name;

        private bool m_enabled = true;
        private List<GameObject> m_children = new List<GameObject>();
        private GameObject m_parent = null;
        private Scene m_scene = null;

        private List<Behaviour> m_behaviours = new List<Behaviour>();
        private Dictionary<Type, int> m_behaviourTypes = new Dictionary<Type, int>();

        public bool enabled
        {
            get { return m_enabled; }
            set
            {
                m_enabled = value;
                if (m_enabled)
                {
                    SendMessage("OnEnable");
                }
                else
                {
                    SendMessage("OnDisable");
                }
            }
        }

        public GameObject parent
        {
            get { return m_parent; }
            set
            {
                if (m_parent != null)
                {
                    m_parent.m_children.Remove(this);
                    m_parent = null;
                }
                m_parent = value;
                if (value != null)
                {
                    m_parent.m_children.Add(this);
                }
            }
        }

        public List<GameObject> children { get { return m_children; } }

        public GameObject()
        {
        }

        public void SendMessage(string methodName, bool propegateHierarchy = true, object[] args = null)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.IgnoreReturn;

            if (args == null) { args = new object[0]; }

            foreach (Behaviour behaviour in m_behaviours)
            {
                Type type = behaviour.GetType();
                type.InvokeMember(methodName, flags, Type.DefaultBinder, behaviour, args);
            }

            if (propegateHierarchy)
            {
                foreach (GameObject child in m_children)
                {
                    child.SendMessage(methodName, propegateHierarchy, args);
                }
            }
        }

        public void Update()
        {
            if (!m_enabled) { return; }

            foreach (Behaviour behaviour in m_behaviours)
            {
                behaviour.Update();
            }

            foreach (GameObject child in m_children)
            {
                child.Update();
            }
        }

        public T AddBehaviour<T>(T behaviour) where T : Behaviour
        {
            if (m_behaviourTypes.ContainsKey(behaviour.GetType()))
            {
                return GetBehaviour<T>();
            }

            m_behaviourTypes.Add(behaviour.GetType(), m_behaviours.Count);
            m_behaviours.Add(behaviour);
            return behaviour;
        }

        public T AddBehaviour<T>() where T : Behaviour, new()
        {
            return AddBehaviour(new T());
        }

        public T GetBehaviour<T>() where T : Behaviour
        {
            if (!m_behaviourTypes.ContainsKey(typeof(T)))
            {
                return null;
            }
            
            return (T)m_behaviours[m_behaviourTypes[typeof(T)]];
        }

        public void RemoveBehaviour<T>() where T : Behaviour
        {
            if (!m_behaviourTypes.ContainsKey(typeof(T)))
            {
                return;
            }

            Type lastType = m_behaviours[m_behaviours.Count - 1].GetType();
            m_behaviours[m_behaviourTypes[typeof(T)]] = m_behaviours[m_behaviours.Count - 1];
            m_behaviourTypes[lastType] = m_behaviourTypes[typeof(T)];

            m_behaviourTypes.Remove(typeof(T));
            m_behaviours.RemoveAt(m_behaviours.Count - 1);
        }

        public void Destroy()
        {
            if (!m_scene.Contains(this)) return;

            SendMessage("OnDestroy", false);

            //detach all children
            while (m_children.Count > 0)
            {
                GameObject child = m_children[0];
                if (child != null) child.Destroy();
            }
            //detatch from parent
            if (parent != null) parent.RemoveChild(this);
            //remove from scene
            if (m_scene != null) m_scene.Remove(this);
        }

        public void Render(GLContext glContext)
        {
            if (!m_enabled) { return; }

            glContext.PushMatrix(matrix);

            foreach (Behaviour behaviour in m_behaviours)
            {
                behaviour.Render(glContext);
            }

            foreach (GameObject child in m_children)
            {
                child.Render(glContext);
            }

            glContext.PopMatrix();
        }

        public void AddChild(GameObject child)
        {
            child.parent = this;
        }

        public void RemoveChild(GameObject child)
        {
            if (child.parent == this)
            {
                child.parent = null;
            }
        }

        public bool HasChild(GameObject gameObject)
        {
            GameObject par = gameObject;
            while (par != null)
            {
                if (par == this) return true;
                par = par.parent;
            }
            return false;
        }

        public override string ToString()
        {
            return "[" + this.GetType().Name + "::" + name + "]";
        }

    }
}

