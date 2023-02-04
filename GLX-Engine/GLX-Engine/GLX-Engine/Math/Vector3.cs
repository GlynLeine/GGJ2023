using System;
using GLXEngine;
using static GLXEngine.Mathf;

namespace GLXEngine.Math
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        static readonly Vector3 upVec = new Vector3(0, 1, 0);
        static readonly Vector3 rightVec = new Vector3(1, 0, 0);
        static readonly Vector3 forwardVec = new Vector3(0, 0, 1);
        static readonly Vector3 oneVec = new Vector3(1, 1, 1);
        static readonly Vector3 zeroVec = new Vector3(0, 0, 0);

        public static Vector3 up { get { return upVec; } }
        public static Vector3 right { get { return rightVec; } }
        public static Vector3 forward { get { return forwardVec; } }
        public static Vector3 one { get { return oneVec; } }
        public static Vector3 zero { get { return zeroVec; } }

        //------------------------------------------------------------------------------------------------------------------------
        //														Constructors
        //------------------------------------------------------------------------------------------------------------------------
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 source)
        {
            this.x = source.x;
            this.y = source.y;
            this.z = source.z;
        }
        public Vector3(Vector3i source)
        {
            this.x = source.x;
            this.y = source.y;
            this.z = source.z;
        }


        //------------------------------------------------------------------------------------------------------------------------
        //												    Randomised vectors
        //------------------------------------------------------------------------------------------------------------------------
        public static Vector3 Random(float a_minX, float a_maxX, float a_minY, float a_maxY, float a_minZ, float a_maxZ)
        { return new Vector3(Utils.Random(a_minX, a_maxX), Utils.Random(a_minY, a_maxY), Utils.Random(a_minZ, a_maxZ)); }

        public static Vector3 Random(float a_minValue, float a_maxValue)
        { return new Vector3(Utils.Random(a_minValue, a_maxValue), Utils.Random(a_minValue, a_maxValue), Utils.Random(a_minValue, a_maxValue)); }

        public static Vector3 RandomWithLength(float a_minLength, float a_maxLength)
        { return new Vector3(Utils.Random(0f, 1f), Utils.Random(0f, 1f), Utils.Random(0f, 1f)).SetMagnitude(Utils.Random(a_minLength, a_maxLength)); }

        //public static Vector3 RandomFromAngle(float a_minAngle, float a_maxAngle, Vector3 up)
        //{ return new Vector3(Utils.Random(a_minAngle, a_maxAngle)); }


        //------------------------------------------------------------------------------------------------------------------------
        //												        ToString()
        //------------------------------------------------------------------------------------------------------------------------
        override public string ToString() { return $"[Vector3 {x}, {y}, {z}]"; }


        //------------------------------------------------------------------------------------------------------------------------
        //												   Secundary accessors
        //------------------------------------------------------------------------------------------------------------------------
        public float magnitude
        {
            get { return Sqrt((x * x) + (y * y) + (z * z)); }
            set
            {
                this = normal * value;
            }
        }
        public float sqrMagnitude
        {
            get { return (x * x) + (y * y) + (z * z); }
        }
        public float angle
        {
            get
            {
                if (magnitude != 0)
                {
                    float temp = Acos(Dot(new Vector3(1, 0, 0)) / magnitude) * 180f / PI;
                    if (y < 0)
                        temp *= -1f;
                    return temp;
                }
                else
                    return 0;
            }
            set
            {
                this = normal * value;
            }
        }
        public Vector3 normal { get { return x != 0 || y != 0 || z != 0 ? this / magnitude : new Vector3(); } }


        //------------------------------------------------------------------------------------------------------------------------
        //												   Vector functions
        //------------------------------------------------------------------------------------------------------------------------
        public float Dot(Vector3 b) { return x * b.x + y * b.y + z * b.z; }
        public static float Distance(Vector3 a, Vector3 b) { return Abs((a - b).magnitude); }
        public static Vector3 Clamp(Vector3 a_source, Vector3 a_min, Vector3 a_max) { return new Vector3(Mathf.Clamp(a_source.x, a_min.x, a_max.x), Mathf.Clamp(a_source.y, a_min.y, a_max.y), Mathf.Clamp(a_source.z, a_min.z, a_max.z)); }
        public static Vector3 Null() { return new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); }
        public Vector3 Normalize() { return this /= magnitude; }
        public Vector3 SetMagnitude(float a_newMag) { magnitude = a_newMag; return this; }
        public Vector3 SetAngle(float a_newAngle) { angle = a_newAngle; return this; }
        public Vector3 Rotate(float a_rotation) { angle += a_rotation; return this; }
        public Vector3 Clamp(Vector3 a_min, Vector3 a_max) { x = Mathf.Clamp(x, a_min.x, a_max.x); y = Mathf.Clamp(y, a_min.y, a_max.y); return this; }
        public float Distance(Vector3 b) { return Abs((this - b).magnitude); }

        public override bool Equals(object obj)
        {
            return this == (Vector3)(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //												      Operators
        //------------------------------------------------------------------------------------------------------------------------
        public static implicit operator float[](Vector3 a) { return new float[] { a.x, a.y }; }
        public static implicit operator Vector3(float[] a)
        { return a == null ? Null() : (a.Length < 3 ? throw new Exception("Float array has fewer numbers than the array.") : new Vector3(a[0], a[1], a[2])); }

        public static implicit operator Vector3i(Vector3 a) { return new Vector3i(a); }
        public static implicit operator Vector3(Vector3i a) { return new Vector3(a); }

        public static Vector3 operator -(Vector3 a, Vector3 b) { return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z); }
        public static Vector3 operator +(Vector3 a, Vector3 b) { return new Vector3(a.x + b.x, a.y + b.y, a.z - b.z); }
        public static Vector3 operator *(Vector3 a, Vector3 b) { return new Vector3(a.x * b.x, a.y * b.y, a.z - b.z); }
        public static Vector3 operator /(Vector3 a, Vector3 b) { return new Vector3((b.x == 0 ? 0 : (a.x / b.x)), (b.y == 0 ? 0 : (a.y / b.y)), (b.z == 0 ? 0 : (a.z / b.z))); }

        public static Vector3 operator -(Vector3 a) { return new Vector3(-a.x, -a.y, -a.z); }

        public static Vector3 operator *(Vector3 a, float b) { return new Vector3(a.x * b, a.y * b, a.z * b); }
        public static Vector3 operator *(float a, Vector3 b) { return new Vector3(b.x * a, b.y * a, b.z * a); }
        public static Vector3 operator /(Vector3 a, float b) { return new Vector3((b == 0 ? 0 : (a.x / b)), (b == 0 ? 0 : (a.y / b)), (b == 0 ? 0 : (a.z / b))); }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return (a.x <= b.x + 0.0001f && a.x >= b.x - 0.0001f) && (a.y <= b.y + 0.0001f && a.y >= b.y - 0.0001f) && (a.z <= b.z + 0.0001f && a.z >= b.z - 0.0001f);
        }
        public static bool operator !=(Vector3 a, Vector3 b) { return !(a == b); }


        //------------------------------------------------------------------------------------------------------------------------
        //												   Interpolation
        //------------------------------------------------------------------------------------------------------------------------
        public Vector3 Lerp(Vector3 a_target, float a_scalar) { return this += (a_target - this) * a_scalar; }
        public static Vector3 Lerp(Vector3 a_source, Vector3 a_target, float a_scalar) { return a_source + (a_target - a_source) * a_scalar; }
        public Vector3 Slerp(Vector3 a_target, float a_scalar) { this.angle += (a_target.angle - this.angle) * a_scalar; return this; }
        public static Vector3 Slerp(Vector3 a_source, Vector3 a_target, float a_scalar) { Vector3 temp = new Vector3(a_source); temp.angle += (a_target.angle - a_source.angle) * a_scalar; return temp; }
    }




    //------------------------------------------------------------------------------------------------------------------------
    //														Vector3i
    //------------------------------------------------------------------------------------------------------------------------
    public struct Vector3i
    {
        public int x;
        public int y;
        public int z;

        public Vector3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3i(Vector3i source)
        {
            this.x = source.x;
            this.y = source.y;
            this.z = source.z;
        }

        public Vector3i(Vector3 source)
        {
            this.x = Round(source.x);
            this.y = Round(source.y);
            this.z = Round(source.z);
        }

        override public string ToString() { return $"[Vector3:{x}, {y}, {z}]"; }

        public float magnitude
        {
            get { return Sqrt((x * x) + (y * y) + (z * z)); }
            set
            {
                this = normal * value;
            }
        }
        public int sqrMagnitude
        {
            get { return (x * x) + (y * y) + (z * z); }
        }
        public float angle
        {
            get
            {
                if (magnitude != 0)
                {
                    float temp = Acos(Dot(new Vector3(1, 0, 0)) / magnitude) * 180f / PI;
                    if (y < 0) temp *= -1f;
                    return temp;
                }
                else
                    return 0;
            }
            set
            {
                this = normal * value;
            }
        }
        public Vector3i normal { get { return x != 0 || y != 0 || z != 0 ? this / magnitude : new Vector3i(); } }


        public float Dot(Vector3 b) { return x * b.x + y * b.y; }
        public static float Distance(Vector3 a, Vector3 b) { return Abs((a - b).magnitude); }
        public Vector3i Normalize() { return this /= magnitude; }
        public Vector3i SetMagnitude(float a_newMag) { this.magnitude = a_newMag; return this; }
        public Vector3i SetAngle(float a_newAngle) { this.angle = a_newAngle; return this; }

        public override bool Equals(object obj)
        {
            return this == (Vector3i)(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator int[](Vector3i a) { return new int[] { a.x, a.y, a.z }; }
        public static implicit operator Vector3i(int[] a)
        { if (a.Length < 2) throw new Exception("Float array has fewer numbers than the array."); else return new Vector3i(a[0], a[1], a[2]); }

        public static implicit operator Vector3i(Vector3 a) { return new Vector3i(a); }
        public static implicit operator Vector3(Vector3i a) { return new Vector3(a); }

        public static Vector3i operator -(Vector3i a, Vector3i b) { return new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z); }
        public static Vector3i operator +(Vector3i a, Vector3i b) { return new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z); }
        public static Vector3i operator *(Vector3i a, Vector3i b) { return new Vector3i(a.x * b.x, a.y * b.y, a.z + b.z); }
        public static Vector3i operator /(Vector3i a, Vector3i b) { return new Vector3i(Round(b.x == 0 ? 0 : (a.x / b.x)), Round(b.y == 0 ? 0 : (a.y / b.y)), Round(b.z == 0 ? 0 : (a.z / b.z))); }

        public static Vector3i operator *(Vector3i a, float b) { return new Vector3i(Round(a.x * b), Round(a.y * b), Round(a.z * b)); }
        public static Vector3i operator *(float a, Vector3i b) { return new Vector3i(Round(b.x * a), Round(b.y * a), Round(b.z * a)); }
        public static Vector3i operator /(Vector3i a, float b) { return new Vector3i(Round(b == 0 ? 0 : (a.x / b)), Round(b == 0 ? 0 : (a.y / b)), Round(b == 0 ? 0 : (a.z / b))); }

        public static bool operator ==(Vector3i a, Vector3i b) { return a.x == b.x && a.y == b.y; }
        public static bool operator !=(Vector3i a, Vector3i b) { return !(a == b); }

        public Vector3i Lerp(Vector3i a_target, float a_scalar) { return this += (a_target - this) * a_scalar; }
        public static Vector3i Lerp(Vector3i a_source, Vector3i a_target, float a_scalar) { return a_source + (a_target - a_source) * a_scalar; }
        public Vector3i Slerp(Vector3i a_target, float a_scalar) { this.angle += (a_target.angle - this.angle) * a_scalar; return this; }
        public static Vector3i Slerp(Vector3i a_source, Vector3i a_target, float a_scalar) { Vector3i temp = new Vector3i(a_source); temp.angle += (a_target.angle - a_source.angle) * a_scalar; return temp; }
    }
}
