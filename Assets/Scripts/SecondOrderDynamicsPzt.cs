using UnityEngine;


public class SecondOrderDynamicsPzt
{
    private Vector3 _xp, _xd;
    private Vector3 _yd, _y;
    private readonly float _w, _z, _d, _k1, _k2, _k3;

    public SecondOrderDynamicsPzt(float f, float z, float r, Vector3 x0)
    {
        _w = 2 * Mathf.PI * f;
        _z = z;
        _d = _w * Mathf.Sqrt(Mathf.Abs(z * z - 1));
        
        _k1 = z / (Mathf.PI * f);
        _k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        _k3 = r * z / (2 * Mathf.PI * f);

        _xp = x0;
        _y = x0;
        _yd = Vector3.zero;
    }

    public Vector3 Update(float T, Vector3 x)
    {
        _xd = (x - _xp) / T;
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
        _y = _y + T * _yd;
        _yd = _yd + T * (x + _k3 * _xd - _y - k1Stable * _yd) / k2Stable;

        return _y;
    }
}
