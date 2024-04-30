using UnityEngine;


namespace Quaternions
{
    public class QuaternionOperations
    {
        public static Quaternion QPlus(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Quaternion QDif(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Quaternion QMul(Quaternion a, Quaternion b)
        {
            var ab = new Vector3(a.x, a.y, a.z);
            var bb = new Vector3(b.x, b.y, b.z);

            var qb = a.w * ab + b.w * bb + Vector3.Cross(ab, bb);
            var qa = a.w * b.w - Vector3.Dot(ab, bb);
            return new Quaternion(qb.x, qb.y, qb.z, qa);
        }

        public static Quaternion QMul(Quaternion a, float b)
        {
            return new Quaternion(b * a.x, b * a.y, b * a.z, b * a.w);
        }

        public static Quaternion QConj(Quaternion a)
        {
            return new Quaternion(-a.x, -a.y, -a.z, a.w);
        }

        public static float QNorm(Quaternion a)
        {
            return Mathf.Sqrt(Mathf.Pow(a.x, 2) + Mathf.Pow(a.y, 2) + Mathf.Pow(a.z, 2) + Mathf.Pow(a.w, 2));
        }


        public static Quaternion QExp(Quaternion a)
        {
            var ab = new Vector3(a.x, a.y, a.z);
            var abNorm = Vector3.Normalize(ab);

            var qw = Mathf.Exp(a.w) * Mathf.Cos(ab.magnitude);
            var qb = abNorm * (Mathf.Exp(a.w) * Mathf.Sin(ab.magnitude));
            return new Quaternion(qb.x, qb.y, qb.z, qw);
        }

        public static Quaternion QLn(Quaternion a)
        {
            var ab = new Vector3(a.x, a.y, a.z);
            var abNorm = Vector3.Normalize(ab);

            var qw = Mathf.Log(QNorm(a));
            var qb = abNorm * Mathf.Acos(a.w / QNorm(a));
            return new Quaternion(qb.x, qb.y, qb.z, qw);
        }

        public static Quaternion QInv(Quaternion a)
        {
            return QMul(QConj(a), 1 / QNorm(a));
        }

        public static float QDot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }
    }
}
