using UnityEngine;
using static Quaternions.QuaternionOperations;

public class SecondOrderDynamicsRotation
{
    private Quaternion _xp, _xd;
    private Quaternion _yd, _y;
    private readonly float _k1, _k2, _k3;

    public SecondOrderDynamicsRotation(float f, float z, float r, Quaternion x0)
    {
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
        _xd = QMul(QLn(QMul(x, QConj(_xp))), _xp);
        _xp = x;
        
        
        float k2Stable = Mathf.Max(_k2, T * T / 2 + T * _k1 / 2, T * _k1);
        // _y = Quaternion.Normalize(QMul(QExp(QMul(_yd, T)), _y));
        _y = Quaternion.Normalize(QMul(QExp(QMul(QMul(_yd, QConj(_y)), T)), _y));
        _yd = Quaternion.Normalize(QPlus(_yd, QMul(QPlus(QPlus(QMul(QLn(x), QInv(_y)), QMul(_xd, _k3)), QMul(_yd, -_k1)), T/ k2Stable)));

        return _y;
    }
}
