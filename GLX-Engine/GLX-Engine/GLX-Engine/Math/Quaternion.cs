using System;
using GLXEngine;
using static GLXEngine.Mathf;

namespace GLXEngine.Math
{
    public struct Quaternion
    {
        public float m_w;
        public float m_x;
        public float m_y;
        public float m_z;

        public Quaternion(float w = 1, float x = 0,float y = 0,float z =0)
        {
            m_w = w;
            m_x = x;
            m_y = y;
            m_z = z;
        }

        public Quaternion(Quaternion quat)
        {
            m_w = quat.m_w;
            m_x = quat.m_x;
            m_y = quat.m_y;
            m_z = quat.m_z;
        }

        public float magnitude
        {
            get { return Mathf.Sqrt(Dot(this)); }
        }

        public Quaternion normal
        {
            get 
            {
                float mag = magnitude;
                return new Quaternion(m_w/mag, m_x/mag,m_y/mag,m_z/mag);
            }
        }

        public Quaternion Normalize(Quaternion q)
        {
            float mag = Mathf.Sqrt(Dot(q));

            if (mag < Mathf.Epsilon)
                return new Quaternion();

            return new Quaternion(q.m_x / mag, q.m_y / mag, q.m_z / mag, q.m_w / mag);
        }

        public float Dot(Quaternion q) { return m_x * q.m_x + m_y * q.m_y + m_z * q.m_z; }
    }
}
