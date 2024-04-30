using UnityEngine;
using static Quaternions.QuaternionOperations;


public class SecondOrderDynamicsRotationPzt
{
    private Quaternion _xp, _xd;
    private Quaternion _yd, _y;
    private readonly float _w, _z, _d, _k1, _k2, _k3;

    public SecondOrderDynamicsRotationPzt(float f, float z, float r, Quaternion x0)
    {
        _w = 2 * Mathf.PI * f;
        _z = z;
        _d = _w * Mathf.Sqrt(Mathf.Abs(z * z - 1));
        
        _k1 = z / (Mathf.PI * f);
        _k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        _k3 = r * z / (2 * Mathf.PI * f);

        _xp = x0;
        _y = x0;
        _yd = new Quaternion(0, 0, 0, 0);
    }



    public Quaternion Update(float T, Quaternion x)
    {
        // ẋ[n+1] = Log(x[n+1]x⁻¹[n]) / T
        // y[n+1] = exp(Tẏ[n])y[n]
        // ẏ[n+1] = ẏ[n] + T(Log(x[n+1]y⁻¹[n+1]) + k₃ẋ[n+1] - k₁ẏ[n]) / k₂
     
        _xd = Quaternion.Normalize(QMul(QLn(QMul(x, QInv(_xp))), 1 / T));
        _xp = x;
        
        float k1Stable, k2Stable;

        if (_w * T < _z)
        {
            k1Stable = _k1;
            k2Stable = Mathf.Max(_k2, T * T / 2 + T * _k1 / 2, T * _k1);
        }
        else
        {
            float t1 = Mathf.Exp(-_z * _w * T);
            float alpha = 2 * t1 * (_z <= 1 ? Mathf.Cos(T * _d) : (float)System.Math.Cosh(T * _d));
            float beta = t1 * t1;
            float t2 = T / (1 + beta - alpha);
            k1Stable = (1 - beta) * t2;
            k2Stable = T * t2;
        }
        _y = Quaternion.Normalize(QMul(QExp(QMul(_yd, T)), _y));
        _yd = Quaternion.Normalize(QPlus(_yd, QMul(QPlus(QPlus(QMul(QLn(x), QInv(_y)), QMul(_xd, _k3)), QMul(_yd, -k1Stable)), T/ k2Stable)));

        return _y;
    }
}
